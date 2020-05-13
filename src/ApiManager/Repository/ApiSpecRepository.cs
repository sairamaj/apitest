using System.IO;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace ApiManager.Repository
{
	class ApiSpecRepository : IApiSpecRepository
	{
		public OpenApiDocument GetFromOpenApiSpecFile(string fileName)
		{
			using (var fs = new FileStream(fileName, FileMode.Open))
			{
				return  new OpenApiStreamReader().Read(fs, out var diagnostic);
			}
		}
	}
}
