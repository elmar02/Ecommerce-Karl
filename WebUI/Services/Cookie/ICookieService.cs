namespace WebUI.Services.Cookie
{
    public interface ICookieService
    {
        string GetCookieValue(string key);
        void SetCookie(string key, string value, int? expireDays = null);
        void RemoveCookie(string key);
    }
}
