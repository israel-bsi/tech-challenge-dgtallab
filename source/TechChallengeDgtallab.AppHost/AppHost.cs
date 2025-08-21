var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("db").WithPgAdmin();

var techChallengeDb = db.AddDatabase("techChallengeDb");

var apiService = builder.AddProject<Projects.TechChallengeDgtallab_ApiService>("apiservice")
    .WithReference(techChallengeDb)
    .WaitFor(techChallengeDb)
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.TechChallengeDgtallab_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
