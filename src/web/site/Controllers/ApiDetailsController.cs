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
        public async Task<IActionResult> Index(string id)
        {
            ApiInfoEntity apiInfo = null;
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            await foreach (var api in this.Repository.GetApiDetails(id))
            {
                apiInfo = api;

                apiInfo.Data = JsonSerializer.Serialize(JsonSerializer.Deserialize<object>(api.Data), options);
                break;
            }

            return View(apiInfo);
        }
    }

}