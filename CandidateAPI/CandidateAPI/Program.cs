using CandidateAPI.Data;
using CandidateAPI.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the memory cache service
builder.Services.AddMemoryCache();

// Register the CandidateRepository
builder.Services.AddScoped<CandidateRepository>();

// Register the CachedCandidateRepository
builder.Services.AddScoped<ICandidateRepository>(provider =>
{
    var innerRepository = provider.GetRequiredService<CandidateRepository>();
    var cache = provider.GetRequiredService<IMemoryCache>();
    return new CachedCandidateRepository(innerRepository, cache);
});

var app = builder.Build();

// Use the exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

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
