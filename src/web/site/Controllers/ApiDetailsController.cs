using Microsoft.AspNetCore.Mvc;
using site.Models;

namespace site.Controllers
{
    public class ApiDetailsController : Controller
    {
        public IActionResult Index(string id)
        {
            System.Console.WriteLine($" Id: {id}");
            var apiInfo = new ApiInfoEntity {
                Data = System.IO.File.ReadAllText(@"c:\temp\test.json")
            };
            return View(apiInfo);
        }
    }

}