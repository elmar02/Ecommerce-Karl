namespace WebUI.Services.Cookie
{
    public class CookieManager : ICookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCookieValue(string key)
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[key];
        }

        public void SetCookie(string key, string value, int? expireDays = null)
        {
            var option = new CookieOptions
            {
                Expires = expireDays.HasValue ? DateTime.Now.AddDays(expireDays.Value) : DateTime.Now.AddYears(1),
            };

            _httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, option);
        }

        public void RemoveCookie(string key)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(key);
        }
    }

}
