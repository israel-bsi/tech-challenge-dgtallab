namespace TechChallengeDgtallab.Core;

public class Configuration
{
    public const int DefaultStatusCode = 200;
    public const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 25;
    public static string ConnectionString { get; set; } = string.Empty;
    public static string CorsPolicyName { get; set; } = "default";

    public const string BackendUrl = "http://localhost:5598";

    public const string FrontendUrl = "http://localhost:5118";
}