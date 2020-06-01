using System.IO;
using ApiManager.Resources.Model;

namespace ApiManager.Resources
{
	static class ResourceEditingHelper
	{
		public static ResourceData CreateNewResource(string parentPath)
		{
			return new ResourceData(Path.Combine(parentPath, "new.json"));
		}
	}
}
