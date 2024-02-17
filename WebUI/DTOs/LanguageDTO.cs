namespace WebUI.DTOs
{
    public class LanguageDTO
    {
        public Dictionary<string, string> Languages { get; set; }
        public string CurrentLangCode { get; set; }
        public string PreviousLink { get; set; }
    }
}
