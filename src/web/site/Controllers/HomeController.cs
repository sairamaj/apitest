using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using site.Models;

namespace site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var runs = new List<RunEntity>(){
                new RunEntity {
                    Name="Apigee",
                    Status= RunStatus.Completed,
                    Apis = new List<ApiInfoEntity>{
                        new ApiInfoEntity{
                            Method = "GET",
                            HttpCode = 200,
                            StatusCode = "Ok",
                            Url  = "https://foo.com/api/oauth"
                        },
                        new ApiInfoEntity{
                            Method = "GET",
                            HttpCode = 400,
                            StatusCode = "Bad Request",
                            Url  = "https://foo.com/api/list"
                        },
                    }
                },
                new RunEntity {Name="Apigee", Status= RunStatus.Running}
            };
            return View(runs);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
