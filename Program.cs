using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ILogService, LogService>(); // AddSingleton to AddScoped (per request) or AddTransient (per usage)
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/userManagement.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.Use(async (context, next) =>
    {
        await next(); // Run the next middleware first

        if (context.Response.StatusCode >= 400)
        {
            var logger = context.RequestServices.GetRequiredService<ILogService>();
            logger.Log($"Security Event: {context.Request.Method} {context.Request.Path} - Status Code: {context.Response.StatusCode}");
        }
    });
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
    app.UseHttpsRedirection();
    app.UseExceptionHandler("/Home/Error");
}

app.MapControllers();

app.Run();