using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using site.Models;
using site.Repository;

namespace site.Controllers
{
    public class ApiDetailsController : Controller
    {
        private readonly ILogger<ApiDetailsController> _logger;

        public ApiDetailsController(
        IAzureRepository repository,
        ILogger<ApiDetailsController> logger)
        {
            Repository = repository;
            _logger = logger;
        }

        public IAzureRepository Repository { get; }
        public async Task<IActionResult> Index(string runId, string apiId)
        {
            System.Console.WriteLine($"env: {runId}:{apiId}");
            var detail = await this.Repository.GetApiDetails(runId, apiId);
            return View(detail);
        }
    }

}