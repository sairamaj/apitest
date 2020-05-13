using Microsoft.OpenApi.Models;

namespace ApiManager.Repository
{
	interface IApiSpecRepository
	{
		OpenApiDocument GetFromOpenApiSpecFile(string fileName);
	}
}
