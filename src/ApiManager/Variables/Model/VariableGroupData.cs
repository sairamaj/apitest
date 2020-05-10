using System.IO;

namespace ApiManager.Asserts.Model
{
	class VariableGroupData
	{
		public VariableGroupData(string name, string fileName)
		{
			Name = name;
			FileName = fileName;
		}

		public string Name { get; }
		public string FileName { get; }
		public string FileWithExtension => Path.GetFileName(this.FileName);
	}
}
