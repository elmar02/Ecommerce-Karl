using Entities.Statics;
using Microsoft.AspNetCore.Mvc;
using WebUI.Services.Cookie;
using WebUI.Services.Language;

namespace WebUI.Controllers
{
    public class LanguageController : Controller
    {
        private readonly ICookieService _cookieService;
        private readonly ILanguageService _languageService;
        public LanguageController(ICookieService cookieService, ILanguageService languageService)
        {
            _cookieService = cookieService;
            _languageService = languageService;
        }

        [HttpPost]
        public IActionResult ChangeLanguage(string langCode, string currentUrl)
        {
            if(LanguageCodes.Codes.ContainsValue(langCode))
            {
                _cookieService.SetCookie("lang", langCode);
            }
            else if(langCode == "default")
            {
                _cookieService.SetCookie("lang", _languageService.GetDeviceLanguage());
            }
            return Redirect(currentUrl);
        }
    }
}
