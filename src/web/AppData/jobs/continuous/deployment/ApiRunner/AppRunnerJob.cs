using System;
using ApiRunner.Repository;
using Microsoft.Azure.WebJobs;

namespace ApiRunner
{

    public class AppRunnerJob
    {
        [Singleton]
        public static void TimerTick([TimerTrigger("0 * * * * *")]TimerInfo myTimer)
        {
            Console.WriteLine($"Hello at {DateTime.UtcNow.ToString()}");
        }

        public static void Start()
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=saiapirunner;AccountKey=PDJPxa+gWk/CXnTJ7YTZuPC1INqTNvQ8PvjgbT3FB3cfg2SqoCX3SWwcs/oc00v+a2obIGeM5V+3JzH/8fOviA==;EndpointSuffix=core.windows.net";
            System.Console.WriteLine("Running apigee...");
            new Runner(new AzureRepository(connectionString)).Run("apigee").Wait();
            System.Console.WriteLine("Running done...");
        }
    }
}   
