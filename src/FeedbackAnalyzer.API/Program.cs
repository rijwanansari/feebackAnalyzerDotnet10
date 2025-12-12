using FeedbackAnalyzer.API.Data;
using FeedbackAnalyzer.API.Hubs;
using FeedbackAnalyzer.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins(builder.Configuration["CorsOrigins"] ?? "https://localhost:5001")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Configure Entity Framework with SQL Server
builder.Services.AddDbContext<FeedbackDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// Register application services
builder.Services.AddScoped<ISentimentAnalysisService, AzureSentimentAnalysisService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();

// Add SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorClient");

app.UseAuthorization();

app.MapControllers();
app.MapHub<FeedbackHub>("/feedbackhub");

app.Run();
