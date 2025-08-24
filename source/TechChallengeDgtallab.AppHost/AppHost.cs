var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("db")
    .WithPgAdmin(opt=>opt.WithHostPort(58678))
    .WithDataVolume("postgres_data");

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
