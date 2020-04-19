using System;
using Microsoft.Azure.WebJobs;

public class AppRunner
{
    [Singleton]
    public static void TimerTick([TimerTrigger("0 * * * * *")]TimerInfo myTimer)
    {
        Console.WriteLine($"Hello at {DateTime.UtcNow.ToString()}");
    }
}