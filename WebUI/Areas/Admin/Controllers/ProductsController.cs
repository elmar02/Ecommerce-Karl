using Business.Abstract;
using Entities.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.Admin.Models;
using WebUI.Services.Language;

namespace WebUI.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Super Admin,Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly string LangCode;
        private readonly ICategoryService _categoryService;
        public ProductsController(IProductService productService, ILanguageService languageService, ICategoryService categoryService)
        {
            _productService = productService;
            LangCode = languageService.GetCurrentLanguage();
            _categoryService = categoryService;
        }

        #region Product List
        public IActionResult Index()
        {
            var result = _productService.GetAllProductsAdmin(LangCode);
            if (!result.Success)
                return Redirect("/error/500");
            return View(result.Data);
        }
        #endregion
        #region Create Product
        public IActionResult Create()
        {
            var result = _categoryService.GetAllSubCategoriesAdmin(LangCode);
            if (result.Success)
                ViewData["SubCategories"] = result.Data;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDTO createProductDTO)
        {
            var result = await _productService.CreateProductAsync(createProductDTO);
            if (!result.Success)
            {
                ViewData["Error"] = result.Message;
                var result1 = _categoryService.GetAllSubCategoriesAdmin(LangCode);
                if (!result1.Success)
                    return Redirect("/error/500");
                ViewData["SubCategories"] = result1.Data;
                return View();
            }
            return Redirect($"/{AdminName.Admin}/products");
        }


        #endregion
        #region Delete Product
        public IActionResult Delete(int id)
        {
            var result = _productService.DeleteProductById(id);
            if (!result.Success)
                return Redirect("/error/500");
            return Redirect($"/{AdminName.Admin}/products");
        }
        #endregion
    }
}
