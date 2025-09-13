using System;
using Aspire.Hosting.Lifecycle;

namespace CS.AppHost;

internal static class Extensions
{
    /// <summary>
    /// Adds a hook to set the ASPNETCORE_FORWARDEDHEADERS_ENABLED environment variable to true for all projects in the application.
    /// </summary>
    public static IDistributedApplicationBuilder AddForwardedHeaders(this IDistributedApplicationBuilder builder)
    {
        builder.Services.TryAddLifecycleHook<AddForwardHeadersHook>();
        return builder;
    }

    private class AddForwardHeadersHook : IDistributedApplicationLifecycleHook
    {
        public Task BeforeStartAsync(DistributedApplicationModel appModel, CancellationToken cancellationToken = default)
        {
            foreach (var p in appModel.GetProjectResources())
            {
                p.Annotations.Add(new EnvironmentCallbackAnnotation(context =>
                {
                    context.EnvironmentVariables["ASPNETCORE_FORWARDEDHEADERS_ENABLED"] = "true";
                }));
            }

            return Task.CompletedTask;
        }
    }

    // public static IResourceBuilder<YarpResource> ConfigureMobileBffRoutes(this IResourceBuilder<YarpResource> builder,
    //     IResourceBuilder<ProjectResource> catalogApi,
    //     IResourceBuilder<ProjectResource> orderingApi,
    //     IResourceBuilder<ProjectResource> identityApi)
    // {
    //     return builder.WithConfiguration(yarp =>
    //     {
    //         var catalogCluster = yarp.AddCluster(catalogApi);

    //         yarp.AddRoute("/catalog-api/api/catalog/items", catalogCluster)
    //             .WithMatchRouteQueryParameter([new() { Name = "api-version", Values = ["1.0", "1", "2.0"], Mode = QueryParameterMatchMode.Exact }])
    //             .WithTransformPathRemovePrefix("/catalog-api");

    //         yarp.AddRoute("/catalog-api/api/catalog/items/by", catalogCluster)
    //             .WithMatchRouteQueryParameter([new() { Name = "api-version", Values = ["1.0", "1", "2.0"], Mode = QueryParameterMatchMode.Exact }])
    //             .WithTransformPathRemovePrefix("/catalog-api");

    //         yarp.AddRoute("/catalog-api/api/catalog/items/{id}", catalogCluster)
    //             .WithMatchRouteQueryParameter([new() { Name = "api-version", Values = ["1.0", "1", "2.0"], Mode = QueryParameterMatchMode.Exact }])
    //             .WithTransformPathRemovePrefix("/catalog-api");

    //         yarp.AddRoute("/catalog-api/api/catalog/items/by/{name}", catalogCluster)
    //             .WithMatchRouteQueryParameter([new() { Name = "api-version", Values = ["1.0", "1"], Mode = QueryParameterMatchMode.Exact }])
    //             .WithTransformPathRemovePrefix("/catalog-api");

    //         yarp.AddRoute("/catalog-api/api/catalog/items/withsemanticrelevance/{text}", catalogCluster)
    //             .WithMatchRouteQueryParameter([new() { Name = "api-version", Values = ["1.0", "1"], Mode = QueryParameterMatchMode.Exact }])
    //             .WithTransformPathRemovePrefix("/catalog-api");

    //         yarp.AddRoute("/catalog-api/api/catalog/items/withsemanticrelevance", catalogCluster)
    //             .WithMatchRouteQueryParameter([new() { Name = "api-version", Values = ["2.0"], Mode = QueryParameterMatchMode.Exact }])
    //             .WithTransformPathRemovePrefix("/catalog-api");

    //         yarp.AddRoute("/catalog-api/api/catalog/items/type/{typeId}/brand/{brandId?}", catalogCluster)
    //             .WithMatchRouteQueryParameter([new() { Name = "api-version", Values = ["1.0", "1"], Mode = QueryParameterMatchMode.Exact }])
    //             .WithTransformPathRemovePrefix("/catalog-api");

    //         yarp.AddRoute("/catalog-api/api/catalog/items/type/all/brand/{brandId?}", catalogCluster)
    //             .WithMatchRouteQueryParameter([new() { Name = "api-version", Values = ["1.0", "1"], Mode = QueryParameterMatchMode.Exact }])
    //             .WithTransformPathRemovePrefix("/catalog-api");

    //         yarp.AddRoute("/catalog-api/api/catalog/catalogTypes", catalogCluster)
    //             .WithMatchRouteQueryParameter([new() { Name = "api-version", Values = ["1.0", "1", "2.0"], Mode = QueryParameterMatchMode.Exact }])
    //             .WithTransformPathRemovePrefix("/catalog-api");

    //         yarp.AddRoute("/catalog-api/api/catalog/catalogBrands", catalogCluster)
    //             .WithMatchRouteQueryParameter([new() { Name = "api-version", Values = ["1.0", "1", "2.0"], Mode = QueryParameterMatchMode.Exact }])
    //             .WithTransformPathRemovePrefix("/catalog-api");

    //         yarp.AddRoute("/catalog-api/api/catalog/items/{id}/pic", catalogCluster)
    //             .WithMatchRouteQueryParameter([new() { Name = "api-version", Values = ["1.0", "1", "2.0"], Mode = QueryParameterMatchMode.Exact }])
    //             .WithTransformPathRemovePrefix("/catalog-api");

    //         // Generic catalog catch-all route
    //         yarp.AddRoute("/api/catalog/{*any}", catalogCluster)
    //             .WithMatchRouteQueryParameter([new() { Name = "api-version", Values = ["1.0", "1", "2.0"], Mode = QueryParameterMatchMode.Exact }]);

    //         // Ordering routes
    //         yarp.AddRoute("/api/orders/{*any}", orderingApi.GetEndpoint("http"))
    //             .WithMatchRouteQueryParameter([new() { Name = "api-version", Values = ["1.0", "1"], Mode = QueryParameterMatchMode.Exact }]);

    //         // Identity routes
    //         yarp.AddRoute("/identity/{*any}", identityApi.GetEndpoint("http"))
    //             .WithTransformPathRemovePrefix("/identity");
    //     });
    // }
}
