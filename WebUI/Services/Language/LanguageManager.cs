using Entities.Statics;
using WebUI.Services.Cookie;

namespace WebUI.Services.Language
{
    public class LanguageManager : ILanguageService
    {
        private readonly ICookieService _cookieService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LanguageManager(ICookieService cookieService, IHttpContextAccessor httpContextAccessor)
        {
            _cookieService = cookieService;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetDeviceLanguage()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext?.Request.Headers.TryGetValue("Accept-Language", out var languages) ?? false)
            {
                var language = languages.ToString()?.Split(',').FirstOrDefault()?.Trim();
                return !string.IsNullOrEmpty(language) ? LanguageCodes.Codes.ContainsValue(language) ? language : LanguageCodes.Codes.First().Value : LanguageCodes.Codes.First().Value;
            }
            return LanguageCodes.Codes.First().Value;
        }

        public string GetCurrentLanguage()
        {
            var language = _cookieService.GetCookieValue("lang");
            if (string.IsNullOrEmpty(language))
            {
                var deviceLang = GetDeviceLanguage();
                _cookieService.SetCookie("lang", deviceLang);
                return deviceLang;
            }
            return language;
        }
    }
}
