using System.Net.Http.Json;
using TechChallengeDgtallab.Core.Extensions;
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

            return await result.ProcessResponseAsync<DepartmentResponse>();
        }

        public async Task<Response<DepartmentResponse>> UpdateAsync(UpdateDepartmentRequest request)
        {
            var result = await _httpClient.PutAsJsonAsync($"/api/v1/departments/{request.Id}", request);

            return await result.ProcessResponseAsync<DepartmentResponse>();
        }

        public async Task<Response<DepartmentResponse>> DeleteAsync(int id)
        {
            var result = await _httpClient.DeleteAsync($"/api/v1/departments/{id}");

            return await result.ProcessResponseAsync<DepartmentResponse>();
        }

        public async Task<Response<DepartmentResponse>> GetByIdAsync(int id)
        {
            var result = await _httpClient.GetAsync($"/api/v1/departments/{id}");

            return await result.ProcessResponseAsync<DepartmentResponse>();
        }

        public async Task<Response<IEnumerable<DepartmentResponse>>> GetDepartmentHierarchyAsync(int id)
        {
            var result = await _httpClient.GetAsync($"/api/v1/departments/hierarchy/{id}");

            return await result.ProcessResponseAsync<IEnumerable<DepartmentResponse>>();
        }

        public async Task<PagedResponse<IEnumerable<DepartmentResponse>>> GetAllAsync(PagedRequest request)
        {
            var url = $"/api/v1/departments?pageNumber={request.PageNumber}&pageSize={request.PageSize}";

            if(!string.IsNullOrEmpty(request.SearchTerm))
                url += $"&searchTerm={request.SearchTerm}";

            var result = await _httpClient.GetAsync(url);
            
            return await result.ProcessPagedResponseAsync<IEnumerable<DepartmentResponse>>();
        }
    }
}