using System;

public class LogService : ILogService
{
    private readonly int _serviceId = new Random().Next(100000, 999999);

    public LogService()
    {
        LogCreation();
    }
    public void Log(string message)
    {
        Console.WriteLine($"Log: {message}");
    }

    private void LogCreation()
    {
        Console.WriteLine($"LogService initialized with ID: {_serviceId}");
    }
}