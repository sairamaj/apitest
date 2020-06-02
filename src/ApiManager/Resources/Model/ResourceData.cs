using System;
using System.Collections.Generic;
using System.IO;

namespace ApiManager.Resources.Model
{
	class ResourceData
	{
		private List<ResourceData> _children = new List<ResourceData>();

		public ResourceData(string fileName, bool isContainer = false)
		{
			this.FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
			this.Name = Path.GetFileNameWithoutExtension(fileName);
			this.IsContainer = isContainer;
		}

		internal string GetData()
		{
			if (File.Exists(this.FileName))
			{
				return File.ReadAllText(this.FileName);
			}

			return string.Empty;
		}

		public string Name { get; }
		public string FileWithExtension => Path.GetFileName(this.FileName); 
		public string FileName { get; }
		public bool IsContainer { get; }

		public string ContainerPath
		{
			get
			{
				return this.IsContainer ? this.FileName : Path.GetDirectoryName(this.FileName);
			}
		}

		public IEnumerable<ResourceData> Children { get { return this._children; } }
		public void Add(ResourceData resourceData)
		{
			this._children.Add(resourceData);
		}

	}
}
