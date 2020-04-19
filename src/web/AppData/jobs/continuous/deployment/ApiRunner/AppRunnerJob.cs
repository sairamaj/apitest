using System;
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
            System.Console.WriteLine("Running apigee...");
            new Runner().Run("apigee").Wait();
            System.Console.WriteLine("Running done...");
        }
    }
}   
