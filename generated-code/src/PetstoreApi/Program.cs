using FluentValidation;
using PetstoreApi.Extensions;
using PetstoreApi.Contracts.Extensions;
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
builder.Services.AddProblemDetails();
// Register validators from Contracts package
builder.Services.AddApiValidators();
builder.Services.AddTransient(typeof(MediatR.IPipelineBehavior<,>), typeof(PetstoreApi.Behaviors.ValidationBehavior<,>));
// Register handlers from Implementation assembly
builder.Services.AddApiHandlers();
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
            context.Response.ContentType = "application/problem+json";
            
            var problemDetails = new Microsoft.AspNetCore.Http.HttpValidationProblemDetails(
                validationException.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    ))
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        else if (exception is Microsoft.AspNetCore.Http.BadHttpRequestException badRequestException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/problem+json";
            
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = badRequestException.Message,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        else if (exception is System.Text.Json.JsonException jsonException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/problem+json";
            
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid JSON",
                Detail = "The request body contains invalid JSON",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";
            
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred",
                Detail = app.Environment.IsDevelopment() ? exception?.Message : "An unexpected error occurred",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };

            await context.Response.WriteAsJsonAsync(problemDetails);
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


// Register all API endpoints from Contracts package
app.AddApiEndpoints();

app.Run();

// Make Program accessible for testing
public partial class Program { }