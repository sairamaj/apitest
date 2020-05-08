using System.IO;

namespace ApiManager.Resources.Model
{
	class ResourceData
	{
		public ResourceData(string name, string fileName)
		{
			Name = name;
			FileName = fileName;
		}

		public string Name { get; }
		public string FileWithExtension => Path.GetFileName(this.FileName); 
		public string FileName { get; }
	}
}
