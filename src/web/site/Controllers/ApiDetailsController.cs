using Microsoft.AspNetCore.Mvc;

namespace site.Controllers
{
    public class ApiDetailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }

}