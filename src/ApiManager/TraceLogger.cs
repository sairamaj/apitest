using System.Diagnostics;

namespace ApiManager
{
	internal static class TraceLogger
	{
		public static void Debug(string message)
		{
			Trace.WriteLine($"[ApiManager-Debug] " + message);
		}

		public static void Error(string message)
		{
			Trace.WriteLine($"[ApiManager-Error] " + message);
		}
	}
}
