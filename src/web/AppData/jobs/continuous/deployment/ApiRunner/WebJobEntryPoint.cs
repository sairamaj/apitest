using System;

public class WebJobEntryPoint
{
    private readonly IWebJobConfiguration _webJobConfiguration;

    public WebJobEntryPoint(IWebJobConfiguration webJobConfiguration)
    {
        _webJobConfiguration = webJobConfiguration;
    }

    public void Run()
    {
        Console.WriteLine(_webJobConfiguration.Message);
    }
}