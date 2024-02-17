using Business.Abstract;
using Entities.DTOs.CategoryDTOs;
using Entities.Statics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.Admin.Models;
using WebUI.Services.Language;

namespace WebUI.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = "Super Admin,Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly string LangCode;
        public CategoriesController(ICategoryService categoryService, ILanguageService languageService)
        {
            _categoryService = categoryService;
            LangCode = languageService.GetCurrentLanguage();
        }

        #region Category List
        public IActionResult Index()
        {
            var result = _categoryService.GetAllCategoriesAdmin(LangCode);
            if (!result.Success)
                return Redirect("error/500");
            return View(result.Data);
        }

        #endregion
        #region Create Category
        public IActionResult Create()
        {
            var result = _categoryService.GetAllSubCategoriesAdmin(LangCode);
            if (!result.Success)
                return Redirect("/error/500");
            ViewData["SubCategories"] = result.Data;
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryDTO createCategoryDTO) 
        {
            var result = _categoryService.CreateCategory(createCategoryDTO);
            if (!result.Success)
            {
                ViewData["Error"] = result.Message;
                var result1 = _categoryService.GetAllSubCategoriesAdmin(LangCode);
                if (!result1.Success)
                    return Redirect("/error/500");
                ViewData["SubCategories"] = result1.Data;
                return View(createCategoryDTO);
            }
            return Redirect($"/{AdminName.Admin}/categories");
        }
        #endregion
        #region Delete Category
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result = _categoryService.DeleteCategory(id);
            if (!result.Success)
                return Redirect("/error/500");
            return Redirect($"/{AdminName.Admin}/categories");
        }
        #endregion
        #region Edit Catgeory
        public IActionResult Edit(int id)
        {
            var result = _categoryService.GetCategoryAdmin(id);
            if (!result.Success)
                return Redirect("/error/404");
            ViewData["SubCategories"] = _categoryService.GetAllSubCategoriesAdmin(LangCode).Data
                                        .Where(x => x.Id != id).ToList();
            return View(result.Data);
        }

        [HttpPost]
        public IActionResult Edit(EditCategoryDTO editCategoryDTO)
        {
            var result = _categoryService.EditCategory(editCategoryDTO);
            if (!result.Success)
            {
                var result1 = _categoryService.GetCategoryAdmin(editCategoryDTO.Id);
                if (!result1.Success)
                    return Redirect("/error/404");
                ViewData["SubCategories"] = _categoryService.GetAllSubCategoriesAdmin(LangCode).Data
                                            .Where(x=>x.Id != editCategoryDTO.Id).ToList();
                ViewData["Error"] = result.Message;
                return View(result1.Data);
            }
            return Redirect($"/{AdminName.Admin}/categories");
        }
        #endregion
    }
}
