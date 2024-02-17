using Microsoft.AspNetCore.Mvc;
namespace WebUI.Controllers
{
    public class ErrorController : Controller
    {
        [Route("error/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            ViewData["ErrorCode"] = statusCode;
            switch (statusCode)
            {
                case 404:
                    ViewData["ErrorTitle"] = "Not Found";
                    ViewData["ErrorDesc"] = "The page you're trying to reach isn't available";
                    break;
                case 500:
                    ViewData["ErrorTitle"] = "Internal Error";
                    ViewData["ErrorDesc"] = "Sorry, something went wrong on our end.";
                    break;
                default:
                    ViewData["ErrorTitle"] = "Unexpected Error";
                    ViewData["ErrorDesc"] = "Unexpected error occured!";
                    break;
            }
            return View();
        }
    }
}
