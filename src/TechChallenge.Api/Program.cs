using TechChallenge.API.Middlewares;
using TechChallenge.Infrastructure.Data.Context;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TechChallenge.Infrastructure.Data;
using TechChallenge.Application.Commands.CreateUser;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// MediatR - CQRS
// builder.Services.AddMediatR(cfg =>
//     cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
    
builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly); // Application assembly
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); // API assembly
});

// FluentValidation
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddFluentValidationAutoValidation();

// Infrastructure - Database and Repositories
builder.Services.AddInfrastructure(builder.Configuration);

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "TechChallenge API",
        Version = "v1",
        Description = "API para gerenciamento de usuários",
        Contact = new()
        {
            Name = "TechChallenge Team",
            Email = "team@techchallenge.com"
        }
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Database Migration and Seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        
        // Aplicar migrations automaticamente em Development
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
        c.RoutePrefix = string.Empty; // Swagger na raiz (http://localhost:5000)
    });
}

// Global Exception Handler Middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
