using Quizzz.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy before controllers!
var corsPolicyName = "AllowQuizzzFrontEnd";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        policy
            .WithOrigins("http://localhost:3000") // Next.js development server
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<IQuizRepository, QuizRepository>();
builder.Services.AddSingleton<IQuizRunRepository, QuizRunRepository>();

var app = builder.Build();

app.UseCors(corsPolicyName); // before authorization, swagger and MapControllers!

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
