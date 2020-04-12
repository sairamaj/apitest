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
		public string FileName { get; }
	}
}
