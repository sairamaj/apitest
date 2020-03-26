using System.IO;

namespace ApiManager
{
	internal static class FileHelper
	{
		public static string WriteToTempFile(string content, string ext)
		{
			var tempFileName = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + ext);
			File.WriteAllText(tempFileName, content);
			return tempFileName;
		}

		public static string GetTempFileName(string ext)
		{
			return Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + ext);
		}

		public static void DeleteIfExists(string fileName)
		{
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}
		}
	}
}
