namespace Core.Configuration.Concrete
{
    public static class WWWRootConfiguration
    {
        public static string WebRootPath
        {
            get
            {
                var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                return webRootPath;
            }
        }
    }
}
