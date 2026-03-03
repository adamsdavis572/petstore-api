using FluentValidation;
using PetstoreApi.Configurators;
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

// --- Scan and register application-specific services ---
var serviceConfigurators = typeof(Program).Assembly.GetTypes()
    .Where(t => typeof(IServiceConfigurator).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
    .Select(Activator.CreateInstance)
    .Cast<IServiceConfigurator>();
foreach (var configurator in serviceConfigurators)
    configurator.ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseApiExceptionHandler(app.Environment);

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

// --- Scan and configure application-specific middleware (ordered) ---
var appConfigurators = typeof(Program).Assembly.GetTypes()
    .Where(t => typeof(IApplicationConfigurator).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
    .Select(Activator.CreateInstance)
    .Cast<IApplicationConfigurator>()
    .OrderBy(c => c.Order);
foreach (var configurator in appConfigurators)
    configurator.Configure(app, app.Environment);

// Register all API endpoints (IEndpointFilter instances from DI are applied automatically)
app.AddApiEndpoints();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }))
    .WithName("HealthCheck")
    .WithTags("Health")
    .Produces(200);

app.Run();

// Make Program accessible for testing
public partial class Program { }