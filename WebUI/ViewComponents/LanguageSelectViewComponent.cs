using Entities.Statics;
using Microsoft.AspNetCore.Mvc;
using WebUI.DTOs;
using WebUI.Services.Language;

namespace WebUI.ViewComponents
{
    public class LanguageSelectViewComponent : ViewComponent
    {
        private readonly ILanguageService _languageService;

        public LanguageSelectViewComponent(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var languageDto = new LanguageDTO()
            {
                Languages = LanguageCodes.Codes,
                CurrentLangCode = _languageService.GetCurrentLanguage(),
                PreviousLink = HttpContext.Request.Path + HttpContext.Request.QueryString
            };
            return View("LanguageSelect", languageDto);
        }
    }
}
