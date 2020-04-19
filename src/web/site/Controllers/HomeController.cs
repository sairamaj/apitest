using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using site.Models;
using site.Repository;

namespace site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IAzureRepository repository,
            ILogger<HomeController> logger)
        {
            Repository = repository;
            _logger = logger;
        }

        public IAzureRepository Repository { get; }
        public async Task<IActionResult> Index()
        {
            var runs = new List<RunEntity>();
            await foreach (var run in Repository.GetRuns("apigee"))
            {
                runs.Add(run);
            }

            // Get last run
            var lastRun = runs.OrderByDescending(r=> r.DateTime).FirstOrDefault();
            // Fill apis for the last run
            if( lastRun != null)
            {
                await foreach (var api in Repository.GetApis(lastRun.PartitionKey))
                {
                    lastRun.Apis.Add(api);
                }
            }

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
