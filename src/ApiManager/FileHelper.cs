using System.IO;

namespace ApiManager
{
	internal static class FileHelper
	{
		private const string ApiTestSubPath = "apitester";
		public static string WriteToTempFile(string content, string ext)
		{
			var tempFileName = Path.Combine(GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + ext);
			File.WriteAllText(tempFileName, content);
			return tempFileName;
		}

		public static string WriteToTempFile(string[] lines, string ext)
		{
			var tempFileName = Path.Combine(GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + ext);
			File.WriteAllLines(tempFileName, lines);
			return tempFileName;
		}

		public static string GetTempFileName(string ext)
		{
			return Path.Combine(GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + ext);
		}

		public static void DeleteIfExists(string fileName)
		{
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}
		}

		public static string GetTempPath()
		{
			var apiSubPath = Path.Combine(Path.GetTempPath(), ApiTestSubPath);
			if (!Directory.Exists(apiSubPath))
			{
				Directory.CreateDirectory(apiSubPath);
			}

			return apiSubPath;
		}
	}
}
