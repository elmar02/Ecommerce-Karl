using Business.Abstract;
using Entities.DTOs.StockDTOs;
using Entities.Statics.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.Admin.Models;
using WebUI.Services.Language;

namespace WebUI.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Super Admin,Admin")]
    public class StocksController : Controller
    {
        private readonly IStockService _stockService;
        private readonly IProductService _productService;
        private readonly string LangCode;
        public StocksController(ILanguageService languageService, IStockService stockService, IProductService productService)
        {
            LangCode = languageService.GetCurrentLanguage();
            _stockService = stockService;
            _productService = productService;
        }
        public IActionResult Index()
        {
            var result = _stockService.GetAllAdminStock(LangCode);
            if (!result.Success)
                return Redirect("/error/500");
            return View(result.Data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var products = _productService.GetAllProductName(LangCode);
            if(!products.Success)
                return Redirect("/error/500");
            ViewData["Products"] = products.Data;
            ViewData["Colors"] = Enum.GetNames<Colors>();
            ViewData["Sizes"] = Enum.GetNames<Sizes>();

            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateStockDTO createStockDTO)
        {
            var result = _stockService.CreateStock(createStockDTO);
            if (!result.Success)
            {
                var products = _productService.GetAllProductName(LangCode);
                if (!products.Success)
                    return Redirect("/error/500");
                ViewData["Products"] = products.Data;
                ViewData["Colors"] = Enum.GetNames<Colors>();
                ViewData["Sizes"] = Enum.GetNames<Sizes>();

                return View(createStockDTO);
            }
            return Redirect($"/{AdminName.Admin}/stocks");
        }
    }
}
