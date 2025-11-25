using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<WebApi>("api");

var web = builder.AddViteApp("web", "../../Frontend")
    .WithExternalHttpEndpoints()
    .WithEnvironment("VITE_API_BASE_URL", api.GetEndpoint("http"))
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
