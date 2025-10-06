using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILogService, LogService>(); // AddSingleton to AddScoped (per request) or AddTransient (per usage)

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    /* ---------------------------- Custom middleware --------------------------- */
    app.Use(async (context, next) =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogService>();
        logger.Log($"FIRST Handling request: {context.Request.Method} {context.Request.Path}");
        await next();
        logger.Log($"FIRST Finished handling request.");
    });
    app.Use(async (context, next) =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogService>();
        logger.Log($"SECOND Handling request: {context.Request.Method} {context.Request.Path}");
        await next();
        logger.Log($"SECOND Finished handling request.");
    });
    /* ------------------------ Error handling middleware ----------------------- */
    app.Use(async (context, next) =>
    {
        try
        {
            await next();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Global exception caught: {ex.Message}");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
        }
    });
}
else
{
    app.UseHttpsRedirection(); // for local testing, we disable https redirection
}


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/", (ILogService logger) =>
{
    logger.Log("Hello World!");
    return "Hello World!";
});

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.UseAuthorization();

app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
