using System.Threading.Tasks;
using System.IO;
using Medallion.Shell;
using System;
using System.Text.Json;
using System.Linq;
using ApiRunner.Repository;

namespace ApiRunner
{
    class Runner : IRunner
    {
        public Runner(IAzureRepository repository)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IAzureRepository Repository { get; }

        public async Task Run(string environment)
        {
            // Strt the Run
            var status = RunStatus.Running;
            var run = new RunEntity
            {
                PartitionKey = Guid.NewGuid().ToString(),
                RowKey = environment,
                Status = status,
                DateTime = DateTime.Now
            };

            await this.Repository.Add(run);
            var message = string.Empty;
            try
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
                await ProcessResults(run, resultsPath);
                message = "completed successfully";
                status = RunStatus.Completed;
            }
            catch (Exception e)
            {
                message = e.Message;
                status = RunStatus.Error;
                throw;
            }
            finally
            {
                run.Message = message;
                run.Status = status;
                await this.Repository.Add(run);
            }
        }

        async Task ProcessResults(RunEntity run, string resultsPath)
        {
            foreach (var fileName in Directory.GetFiles(resultsPath, "*api*.json").OrderBy(f => f))
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                System.Console.WriteLine(Path.GetFileName(fileName));
                var data = File.ReadAllText(fileName);
                var apiInfo = JsonSerializer.Deserialize<ApiInfoEntity>(data, options);
                System.Console.WriteLine(apiInfo);
                apiInfo.PartitionKey = run.PartitionKey;
                apiInfo.RowKey = apiInfo.Id.ToString();
                apiInfo.Data = data;
                await this.Repository.Add(apiInfo);
            }
        }
    }
}