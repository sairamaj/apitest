namespace ApiManager.Asserts.Model
{
	class AssertData
	{
		public AssertData(string name, string fileName)
		{
			Name = name;
			FileName = fileName;
		}

		public string Name { get; }
		public string FileName { get; }
	}
}
