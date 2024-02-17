using Business.Abstract;
using Entities.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Mvc;
using WebUI.Services.Language;

namespace WebUI.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService _productService;
        private readonly ILanguageService _languageService;
        public ShopController(IProductService productService, ILanguageService languageService)
        {
            _productService = productService;
            _languageService = languageService;
        }

        public IActionResult Index(FilterDTO filterDTO)
        {
            var result = _productService.GetAllProductsShop(_languageService.GetCurrentLanguage(), filterDTO);
            if (!result.Success)
                return Redirect("/error/500");
            return View(result.Data);
        }
    }
}
