using System.Threading.Tasks;
using System.IO;
using Medallion.Shell;
using System;
using System.Text.Json;
using System.Linq;

namespace ApiRunner
{
    class Runner : IRunner
    {
        public async Task Run(string environment)
        {
            // Get batch file
            System.Console.WriteLine($"runnign : {environment}");
            var envDirectory = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "data", environment);
            var batchProcess = Path.Combine(envDirectory, "runapi.bat");
            System.Console.WriteLine(batchProcess);
            if (!File.Exists(batchProcess))
            {
                throw new ArgumentException($"{batchProcess} not found for {environment}");
            }

            // Run the batch file which produces the results directory.
            var result = await Command.Run(batchProcess, null, options =>
            {
                System.Console.WriteLine($"Setting Working directory: {envDirectory}");
                options.WorkingDirectory(envDirectory);
            }).Task;

            if (!result.Success)
            {
                throw new Exception($"{batchProcess} failed {result.ExitCode}: {result.StandardError}");
            }

            // Proces the results ( results are in environment\results)
            var resultsPath = Path.Combine(Path.Combine(envDirectory, "results"));
            ProcessResults(resultsPath);
        }

        void ProcessResults(string resultsPath)
        {
            foreach (var fileName in Directory.GetFiles(resultsPath, "*api*.json").OrderBy(f => f))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                System.Console.WriteLine(Path.GetFileName(fileName));
                var apiInfo = JsonSerializer.Deserialize<ApiInfoEntity>(File.ReadAllText(fileName), options);
                System.Console.WriteLine(apiInfo);
            }
        }
    }
}