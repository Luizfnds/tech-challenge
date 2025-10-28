using TechChallenge.API;
using TechChallenge.API.Middlewares;
using TechChallenge.Infrastructure.Data.Context;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TechChallenge.Infrastructure.Data;
using TechChallenge.Infrastructure.AWS;
using TechChallenge.Application;

var builder = WebApplication.CreateBuilder(args);

// API Layer - Controllers + Auth + Swagger + CORS
builder.Services.AddApiServices(builder.Configuration);

// FluentValidation Auto Validation
builder.Services.AddFluentValidationAutoValidation();

// Application Layer - CQRS + Validation
builder.Services.AddApplicationServices();

// Infrastructure - Database and Repositories
builder.Services.AddDatabaseInfrastructure(builder.Configuration);

// Infrastructure - AWS
builder.Services.AddAwsInfrastructure(builder.Configuration);

var app = builder.Build();

// Database Migration and Seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        if (app.Environment.IsDevelopment())
        {
            await context.Database.MigrateAsync();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao aplicar migrations ou seed de dados");
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TechChallenge API v1");
        c.RoutePrefix = "swagger"; // ← Swagger em /swagger
    });
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
