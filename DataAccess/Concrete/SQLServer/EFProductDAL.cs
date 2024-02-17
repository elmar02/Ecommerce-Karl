using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.ProductDTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.SQLServer
{
    public class EFProductDAL : EFRepositoryBase<Product, AppDbContext>, IProductDAL
    {
        public bool DeleteProductById(int id)
        {
            try
            {
                using var context = new AppDbContext();
                var product = context.Products
                    .Include(x=>x.Pictures)
                    .Include(x=>x.ProductCategories)
                    .Include(x=>x.ProductLanguages)
                    .Include(x=>x.Specifications)
                    .FirstOrDefault(x => x.Id == id);
                if (product == null)
                    return false;
                Delete(product);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<ProductNameList> GetAllProductName(string langCode)
        {
            using var context = new AppDbContext();
            var products = context.Products
                .Include(x => x.ProductLanguages)
                .Select(x => new ProductNameList()
                {
                    Id = x.Id,
                    Name = x.ProductLanguages.FirstOrDefault(x => x.LangCode == langCode).Name,
                }).ToList();
            return products;
        }

        public List<AdminProductListDTO> GetAllProductsAdmin(string langCode)
        {
            using var context = new AppDbContext();
            var products = context.Products
            .Include(x=>x.ProductLanguages)
            .Select(x => new AdminProductListDTO()
            {
                Id = x.Id,
                DisCount = x.DisCount,
                IsFeatured = x.IsFeatured,
                IsInList = x.IsInList,
                Name = x.ProductLanguages.FirstOrDefault(x => x.LangCode == langCode).Name,
                Price = x.Price,
                Rating = x.Rating,
                Thumbnail = x.ThumbnailUrl
            }).ToList();
            return products;
        }

        public List<ShopProductListDTO> GetAllShopProducts(string langCode)
        {
            using var context = new AppDbContext();
            var products = context.Products
                .Include(x => x.ProductLanguages)
                .Include(x => x.ProductCategories)
                .Select(x => new ShopProductListDTO()
                {
                    Id = x.Id,
                    Description = x.ProductLanguages.FirstOrDefault(x => x.LangCode == langCode).Description,
                    Name = x.ProductLanguages.FirstOrDefault(x => x.LangCode == langCode).Name,
                    Price = x.Price,
                    Rating = x.Rating,
                    DisCount= x.DisCount,
                    SeoUrl = x.SeoUrl,
                    ThumbnailUrl = x.ThumbnailUrl,
                    CategoryIds = x.ProductCategories.Select(x=> x.Id).ToList()
                }).ToList();
            return products;
        }
    }
}
