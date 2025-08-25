using TechChallengeDgtallab.Core.Handler;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.Web.Handlers;

public class DepartmentHandler : IDepartmentHandler
{
    private readonly HttpClient _httpClient;

    public DepartmentHandler(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient(Configuration.HttpClientName);
    }

    public async Task<Response<DepartmentResponse>> AddAsync(EditDepartmentRequest request)
    {
        var result = await _httpClient.PostAsJsonAsync("/api/v1/departments", request);

        return await result.Content.ReadFromJsonAsync<Response<DepartmentResponse>>()
               ?? new Response<DepartmentResponse>(null, 400, "Erro ao criar o departamento");
    }

    public async Task<Response<DepartmentResponse>> UpdateAsync(EditDepartmentRequest request)
    {
        var result = await _httpClient.PutAsJsonAsync("/api/v1/departments", request);

        return await result.Content.ReadFromJsonAsync<Response<DepartmentResponse>>()
               ?? new Response<DepartmentResponse>(null, 400, "Erro ao atualizar o departamento");
    }

    public async Task<Response<DepartmentResponse>> DeleteAsync(int id)
    {
        var result = await _httpClient.DeleteAsync($"/api/v1/departments/{id}");

        return await result.Content.ReadFromJsonAsync<Response<DepartmentResponse>>()
               ?? new Response<DepartmentResponse>(null, 400, "Erro ao deletar o departamento");
    }

    public async Task<Response<DepartmentResponse>> GetByIdAsync(int id)
    {
        var result = await _httpClient.GetAsync($"/api/v1/departments/{id}");

        return await result.Content.ReadFromJsonAsync<Response<DepartmentResponse>>()
               ?? new Response<DepartmentResponse>(null, 400, "Erro ao obter o departamento");
    }

    public async Task<Response<IEnumerable<DepartmentResponse>>> GetDepartmentHierarchyAsync(int id)
    {
        var result = await _httpClient.GetAsync($"/api/v1/departments/hierarchy/{id}");

        return await result.Content.ReadFromJsonAsync<Response<IEnumerable<DepartmentResponse>>>()
               ?? new Response<IEnumerable<DepartmentResponse>>(null, 400, "Erro ao obter a hierarquia do departamento");
    }

    public async Task<PagedResponse<IEnumerable<DepartmentResponse>>> GetAllAsync(PagedRequest request)
    {
        var url = $"/api/v1/departments?pageNumber={request.PageNumber}&pageSize={request.PageSize}";

        return await _httpClient.GetFromJsonAsync<PagedResponse<IEnumerable<DepartmentResponse>>>(url)
            ?? new PagedResponse<IEnumerable<DepartmentResponse>>(null, 400, "Erro ao obter os departamentos");
    }
}