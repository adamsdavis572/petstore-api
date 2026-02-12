using FluentValidation;
using PetstoreApi.Extensions;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
// Configure JSON serialization with enum member support
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new PetstoreApi.Converters.EnumMemberJsonConverterFactory());
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("1.0.1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "OpenAPI Petstore",
        Description = "This is a sample server Petstore server. For this sample, you can use the api key `special-key` to test the authorization filters.",
        Version = "1.0.1"
    });
});

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(PetstoreApi.Behaviors.ValidationBehavior<,>));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline
// Global exception handler for validation and model binding errors
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        if (exception is FluentValidation.ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            
            var errors = validationException.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).ToArray()
                );

            await context.Response.WriteAsJsonAsync(new 
            { 
                error = "Validation failed",
                message = "One or more validation errors occurred",
                errors = errors
            });
        }
        else if (exception is Microsoft.AspNetCore.Http.BadHttpRequestException badRequestException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            
            await context.Response.WriteAsJsonAsync(new 
            { 
                error = "Bad Request",
                message = badRequestException.Message
            });
        }
        else if (exception is System.Text.Json.JsonException jsonException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            
            await context.Response.WriteAsJsonAsync(new 
            { 
                error = "Invalid JSON",
                message = "The request body contains invalid JSON"
            });
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            
            await context.Response.WriteAsJsonAsync(new 
            { 
                error = "Internal Server Error",
                message = app.Environment.IsDevelopment() ? exception?.Message : "An unexpected error occurred"
            });
        }
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/1.0.1/swagger.json", "OpenAPI Petstore 1.0.1");
    });
}

app.UseHttpsRedirection();
app.UseRouting();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }))
    .WithName("HealthCheck")
    .WithTags("Health")
    .Produces(200);


app.MapAllEndpoints();

app.Run();

// Make Program accessible for testing
public partial class Program { }