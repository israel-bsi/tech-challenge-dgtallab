var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.TechChallengeDgtallab_ApiService>("apiservice");

builder.AddProject<Projects.TechChallengeDgtallab_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
