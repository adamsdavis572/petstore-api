using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using PetstoreApi.Endpoints;

namespace PetstoreApi.Contracts.Extensions;

public static class EndpointExtensions
{
    /// <summary>
    /// Registers all API endpoints from the Contracts package.
    /// Any <see cref="IEndpointFilter"/> instances registered in the DI container
    /// are automatically applied to the route group.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The web application for chaining.</returns>
    public static WebApplication AddApiEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/v2");

        foreach (var filter in app.Services.GetServices<IEndpointFilter>())
            group.AddEndpointFilter(filter);

        DefaultApiEndpoints.MapDefaultApiEndpoints(group);
        FakeApiEndpoints.MapFakeApiEndpoints(group);
        PetApiEndpoints.MapPetApiEndpoints(group);
        StoreApiEndpoints.MapStoreApiEndpoints(group);
        UserApiEndpoints.MapUserApiEndpoints(group);

        return app;
    }
}
