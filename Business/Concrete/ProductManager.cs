using Business.Abstract;
using Core.Helper;
using Core.Helper.FileHelper;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete;
using Core.Utilities.Results.Concrete.ErrorResults;
using Core.Utilities.Results.Concrete.SuccessResults;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.ProductDTOs;
using Entities.Statics;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        private readonly IProductDAL _productDAL;
        private readonly IFileService _fileService;
        private readonly IProductLanguageService _productLanguageService;
        private readonly IPictureService _pictureService;
        private readonly ISpecificationService _specificationService;
        private readonly IProductCategoryServices _productCategoryServices;
        public ProductManager(IProductDAL productDAL, IFileService fileService, IProductLanguageService productLanguageService, IPictureService pictureService, ISpecificationService specificationService, IProductCategoryServices productCategoryServices)
        {
            _productDAL = productDAL;
            _fileService = fileService;
            _productLanguageService = productLanguageService;
            _pictureService = pictureService;
            _specificationService = specificationService;
            _productCategoryServices = productCategoryServices;
        }

        public async Task<IResult> CreateProductAsync(CreateProductDTO createProductDTO)
        {
            try
            {
                if (createProductDTO.SubCategoryIds == null)
                    return new ErrorResult("Choose at least one category");
                if (createProductDTO.Price - createProductDTO.Discount <= 0)
                    return new ErrorResult("Result Price cannot be zero or negative");
                var checkNames = _productLanguageService.CheckAllLanguages(createProductDTO.Names);
                if (checkNames.Success)
                    return new ErrorResult("Product name is already taken");


                if (LanguageCodes.Codes.Count < createProductDTO.DefaultLanguage)
                    createProductDTO.DefaultLanguage = 0;

                var defaultProductName = createProductDTO.Names.ElementAtOrDefault(createProductDTO.DefaultLanguage);
                var defaultProductDesc = createProductDTO.Descriptions.ElementAtOrDefault(createProductDTO.DefaultLanguage);
                if (defaultProductName == null || defaultProductDesc == null)
                    return new ErrorResult("Default Language cannot be empty");

                createProductDTO.Names = createProductDTO.Names.Select(x => x ?? defaultProductName).ToList();
                createProductDTO.Descriptions = createProductDTO.Descriptions.Select(x => x ?? defaultProductDesc).ToList();

                if (createProductDTO.Thumbnail == null)
                    return new ErrorResult("Thumbnail cannot be null");

                var thumnailUrl = await _fileService.SaveFileAsync(createProductDTO.Thumbnail, "/uploads/products/");
                var newProduct = new Product
                {
                    DisCount = createProductDTO.Discount,
                    IsInList = createProductDTO.IsInList,
                    IsFeatured = createProductDTO.IsFeatured,
                    Price = createProductDTO.Price,
                    ThumbnailUrl = thumnailUrl,
                    SeoUrl = createProductDTO.Names.First().ConverToSeo(),
                };
                _productDAL.Add(newProduct);
                
                //added language
                var result1 = _productLanguageService.AddProductLanguages(createProductDTO.Names, createProductDTO.Descriptions, newProduct.Id);
                if (!result1.Success) return result1;

                //added photos
                if (createProductDTO.Photos != null)
                {
                    var urls = await _fileService.SaveAllFilesAsync(createProductDTO.Photos, "/uploads/products/");
                    var result2 = _pictureService.AddPictures(urls, newProduct.Id);
                    if(!result2.Success) return result2;
                }

                //added specifications
                if(createProductDTO.Specifications != null)
                {
                    var specs = createProductDTO.Specifications.Where(x => x.Value != null).ToDictionary();
                    var result3 = _specificationService.AddSpecifications(specs, newProduct.Id);
                    if(!result3.Success) return result3;
                }

                //added categories
                var result4 = _productCategoryServices.AddCategoriesToProduct(createProductDTO.SubCategoryIds, newProduct.Id);
                if (!result4.Success) return result4;
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public IResult DeleteProductById(int id)
        {
            var result = _productDAL.DeleteProductById(id);
            return new Result(result);
        }

        public Task<IResult> EditProductAsync(EditProductDTO editProductDTO)
        {
            throw new NotImplementedException();
        }

        public IDataResult<List<ProductNameList>> GetAllProductName(string langCode)
        {
            try
            {
                var products = _productDAL.GetAllProductName(langCode);
                if(products == null)
                    return new ErrorDataResult<List<ProductNameList>>();
                return new SuccessDataResult<List<ProductNameList>>(products);
            }
            catch (Exception)
            {
                return new ErrorDataResult<List<ProductNameList>>();
            }
        }

        public IDataResult<List<AdminProductListDTO>> GetAllProductsAdmin(string langCode)
        {
            try
            {
                var products = _productDAL.GetAllProductsAdmin(langCode);
                return new SuccessDataResult<List<AdminProductListDTO>>(products);
            }
            catch (Exception)
            {
                return new ErrorDataResult<List<AdminProductListDTO>>();
            }
        }

        public IDataResult<List<ShopProductListDTO>> GetAllProductsShop(string langCode, FilterDTO filterDTO)
        {
            try
            {
                var products = _productDAL.GetAllShopProducts(langCode);
                if(filterDTO == null)
                    return new SuccessDataResult<List<ShopProductListDTO>>(products);
                FilterProducts(ref products, filterDTO);
                return new SuccessDataResult<List<ShopProductListDTO>>(products);
            }
            catch (Exception)
            {
                return new ErrorDataResult<List<ShopProductListDTO>>();
            }
        }

        private void FilterProducts(ref List<ShopProductListDTO> shopProducts,FilterDTO filterDTO) 
        {
            //filter by catgeory
            if(filterDTO.CategoryIds!=null)
                if (filterDTO.CategoryIds.Count > 0)
                {
                    var filteredCat = new List<ShopProductListDTO>();
                    foreach (var id in filterDTO.CategoryIds)
                    {
                        filteredCat.AddRange(shopProducts.Where(x => x.CategoryIds.Contains(id)));
                    }
                    shopProducts = filteredCat;
                }

            //filter by min price
            if (filterDTO.MinPrice != 0)
                shopProducts = shopProducts.Where(x => x.Price > filterDTO.MinPrice).ToList();

            //filter by max price
            if (filterDTO.MaxPrice != 10000)
                shopProducts = shopProducts.Where(x => x.Price < filterDTO.MaxPrice).ToList();


        }
    }
}
