using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using PetstoreApi.Endpoints;

namespace PetstoreApi.Contracts.Extensions;

public static class EndpointExtensions
{
    /// <summary>
    /// Registers all API endpoints from the Contracts package.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The endpoint route builder for chaining.</returns>
    public static IEndpointRouteBuilder AddApiEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/v2");
        DefaultApiEndpoints.MapDefaultApiEndpoints(group);
        FakeApiEndpoints.MapFakeApiEndpoints(group);
        PetApiEndpoints.MapPetApiEndpoints(group);
        StoreApiEndpoints.MapStoreApiEndpoints(group);
        UserApiEndpoints.MapUserApiEndpoints(group);
        
        return endpoints;
    }
}
