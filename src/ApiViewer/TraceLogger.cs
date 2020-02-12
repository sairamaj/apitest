using System.Diagnostics;

namespace ApiViewer
{
    internal static class TraceLogger
    {
        public static void Debug(string message)
        {
            Trace.WriteLine($"[ApiViewer-Debug] " + message);
        }

        public static void Error(string message)
        {
            Trace.WriteLine($"[ApiViewer-Error] " + message);
        }

    }
}
