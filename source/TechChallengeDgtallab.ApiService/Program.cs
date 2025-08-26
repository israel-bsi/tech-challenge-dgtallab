using TechChallengeDgtallab.ApiService.Extensions;
using TechChallengeDgtallab.Core;

var builder = WebApplication.CreateBuilder(args);


builder.AddServices();
builder.AddConfiguration();
builder.AddDatabase();
builder.AddDocumentation();
builder.Services.AddCors(
    options => options.AddPolicy(
        Configuration.CorsPolicyName,
        policy => policy
            .WithOrigins(Configuration.BackendUrl, Configuration.FrontendUrl)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
    ));
var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.ApplyDatabaseMigrations();
app.UseSwagger();
app.MapSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TechChallenge API v1");
    c.RoutePrefix = string.Empty;
});
app.UseDeveloperExceptionPage();
app.MapControllers();
app.UseCors(Configuration.CorsPolicyName);

app.Run();