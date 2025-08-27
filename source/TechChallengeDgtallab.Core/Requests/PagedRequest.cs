namespace TechChallengeDgtallab.Core.Requests;

public class PagedRequest
{
    public int PageNumber { get; set; } = Configuration.DefaultPageNumber;
    public int PageSize { get; set; } = Configuration.DefaultPageSize;
    public string SearchTerm { get; set; } = string.Empty;
    public string FilterBy { get; set; } = string.Empty;
}