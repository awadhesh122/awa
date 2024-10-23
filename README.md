CandidateAPI
-Author: Awadhesh t. Gupta -Email: awadheshgupta90@gmail.com -For Org: Sigma Software -Task: Job candidate hub API -Main tech stack: .NET

Overview
CandidateAPI is a web application that provides a REST API for storing and managing information about job candidates. It is built using .NET 8.0, Entity Framework Core, and SQLite.

Technologies Used
.NET 8.0
Entity Framework Core
SQLite
Serilog
xUnit
Moq
Getting Started
Prerequisites
Visual Studio 2022
.NET 8.0 SDK
Project Structure
CandidateAPI
Controllers
CandidatesController.cs
Data
ApplicationDbContext.cs
CandidateRepository.cs
CachedCandidateRepository.cs
ICandidateRepository.cs
Middleware
ExceptionHandlingMiddleware.cs
Models
Candidate.cs
Exceptions
CustomException.cs
Documentation
README.md
Program.cs
appsettings.json
Installation
Get CandidateAPI:

Navigate to the project directory: cd CandidateAPI

Open the solution in Visual Studio: start CandidateAPI.sln

Build and run the project: Press F5 in Visual Studio to build and run the application.

Usage
API Endpoints
Add or Update Candidate
Endpoint: /api/candidates Method: POST Description: Adds a new candidate or updates an existing candidate. Request Body: JSON

{ "firstName": "John", "lastName": "Doe", "phoneNumber": "1234567890", "email": "john.doe@example.com", "preferredCallTime": "Morning", "linkedInProfile": "https://linkedin.com/in/johndoe", "gitHubProfile": "https://github.com/johndoe", "comment": "Test comment" }

Response: 200 OK

Get All Candidates
Endpoint: /api/candidates Method: GET Description: Retrieves all candidates. Response: JSON

[ { "id": 1, "firstName": "John", "lastName": "Doe", "phoneNumber": "1234567890", "email": "john.doe@example.com", "preferredCallTime": "Morning", "linkedInProfile": "https://linkedin.com/in/johndoe", "gitHubProfile": "https://github.com/johndoe", "comment": "Test comment" } ]

Architecture
Project Structure
Controllers: Contains API controllers. Data: Contains database context and repositories. Middleware: Contains custom middleware for exception handling. Models: Contains data models. Exceptions: Contains custom exception classes.

Design Patterns
Repository Pattern: Used to abstract data access logic. Dependency Injection: Used to inject dependencies into controllers and services.

Testing
Unit Tests
Unit tests are written using xUnit and Moq. To run the tests:

Open the Test Explorer in Visual Studio. Click on “Run All” to execute all tests. Test Coverage The unit tests cover the following scenarios:

Adding a new candidate Updating an existing candidate Retrieving a candidate by email Retrieving all candidates Logging Logging is configured using Serilog. Logs are written to a file located at logs/log.txt with daily rolling intervals.

Accessing Logs
To view the logs, open the logs/log.txt file in the project directory.

*************************************************************************
Switching from SQLite to SQL Server
To switch the database from SQLite to SQL Server, follow these steps:

1. Install SQL Server NuGet Packages
Right-click on project in Solution Explorer and select "Manage NuGet Packages".
Search for and install the following packages:
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
2. Update the Database Context Configuration
Open Program.cs and update the database context configuration to use SQL Server instead of SQLite.
Program.cs:

using CandidateAPI.Data;
using Microsoft.EntityFrameworkCore;
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
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

Log.CloseAndFlush();



### Update the Connection String
appsettings.json

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server_name;Database=CandidateDb;User Id=your_username;Password=your_password;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
