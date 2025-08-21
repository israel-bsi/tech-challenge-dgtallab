using TechChallengeDgtallab.ApiService.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.AddServices();
builder.AddConfiguration();
builder.AddDatabase();
builder.AddDocumentation();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.ApplyDatabaseMigrations();
app.MapDefaultEndpoints();
app.UseSwagger();
app.MapSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TechChallenge API v1");
    c.RoutePrefix = string.Empty;
});
app.UseDeveloperExceptionPage();
app.MapControllers();

app.Run();