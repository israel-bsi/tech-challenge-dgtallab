using System.Net.Http.Json;
using TechChallengeDgtallab.Core.Handler;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Requests.Department;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.Web.Handlers
{
    public class DepartmentHandler : IDepartmentHandler
    {
        private readonly HttpClient _httpClient;

        public DepartmentHandler(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient(Configuration.HttpClientName);
        }

        public async Task<Response<DepartmentResponse>> AddAsync(CreateDepartmentRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("/api/v1/departments", request);

            return await result.Content.ReadFromJsonAsync<Response<DepartmentResponse>>()
                   ?? new Response<DepartmentResponse>(null, 400, "Erro ao criar o departamento");
        }

        public async Task<Response<DepartmentResponse>> UpdateAsync(UpdateDepartmentRequest request)
        {
            var result = await _httpClient.PutAsJsonAsync($"/api/v1/departments/{request.Id}", request);

            return await result.Content.ReadFromJsonAsync<Response<DepartmentResponse>>()
                   ?? new Response<DepartmentResponse>(null, 400, "Erro ao atualizar o departamento");
        }

        public async Task<Response<DepartmentResponse>> DeleteAsync(int id)
        {
            var result = await _httpClient.DeleteAsync($"/api/v1/departments/{id}");

            return result.IsSuccessStatusCode 
                ? new Response<DepartmentResponse>(null, 200, "Departamento deletado") 
                : new Response<DepartmentResponse>(null, 400, "Erro ao deletar o departamento");
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

            if(!string.IsNullOrEmpty(request.SearchTerm))
                url += $"&searchTerm={request.SearchTerm}";

            var result = await _httpClient.GetAsync(url);
            if (!result.IsSuccessStatusCode)
            {
                return new PagedResponse<IEnumerable<DepartmentResponse>>(null, (int)result.StatusCode,
                    $"Erro ao obter os departamentos: {result.StatusCode}");
            }
            return await result.Content.ReadFromJsonAsync<PagedResponse<IEnumerable<DepartmentResponse>>>()
                   ?? new PagedResponse<IEnumerable<DepartmentResponse>>(null, 400, "Erro ao obter os departamentos");
        }
    }
}