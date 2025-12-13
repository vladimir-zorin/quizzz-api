using Quizzz.Api.Hubs;
using Quizzz.Api.Repositories;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationInsightsTelemetry();

// Add CORS policy before controllers!
var corsPolicyName = "AllowQuizzzFrontEnd";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        var allowedOrigins = new string[] { "http://localhost:3000" };
        policy
            .WithOrigins(allowedOrigins) // Next.js development server
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowed(origin => allowedOrigins.Contains(origin)); // needed for WebSocket protocol used in SignalR
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<IQuizRepository, QuizRepository>();
builder.Services.AddSingleton<IQuizRunRepository, QuizRunRepository>();

var app = builder.Build();

app.UseCors(corsPolicyName); // before authorization, swagger and MapControllers!

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection(); // this breaks SignalR!

app.UseAuthorization();

app.MapControllers();

// Map the SignalR hub
app.MapHub<QuizRunHub>("/quizrunSignalR"); // This matches the URL in your WebSocket service

app.Run();
