using System.Net.Http.Json;
using TechChallengeDgtallab.Core.Extensions;
using TechChallengeDgtallab.Core.Handler;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Requests.Collaborator;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.Web.Handlers
{
    public class CollaboratorHandler : ICollaboratorHandler
    {
        private readonly HttpClient _httpClient;

        public CollaboratorHandler(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient(Configuration.HttpClientName);
        }
        public async Task<Response<CollaboratorResponse>> AddAsync(CreateCollaboratorRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("/api/v1/collaborators", request);

            return await result.ProcessResponseAsync<CollaboratorResponse>();
        }

        public async Task<Response<CollaboratorResponse>> UpdateAsync(UpdateCollaboratorRequest request)
        {
            var result = await _httpClient.PutAsJsonAsync($"/api/v1/collaborators/{request.Id}", request);

            return await result.ProcessResponseAsync<CollaboratorResponse>();
        }

        public async Task<Response<CollaboratorResponse>> DeleteAsync(int id)
        {
            var result = await _httpClient.DeleteAsync($"/api/v1/collaborators/{id}");

            return await result.ProcessResponseAsync<CollaboratorResponse>();
        }

        public async Task<Response<CollaboratorResponse>> GetByIdAsync(int id)
        {
            var result = await _httpClient.GetAsync($"/api/v1/collaborators/{id}");

            return await result.ProcessResponseAsync<CollaboratorResponse>();
        }

        public async Task<Response<IEnumerable<CollaboratorResponse>>> GetSubordinatesAsync(int managerId)
        {
            var result = await _httpClient.GetAsync($"/api/v1/collaborators/subordinates/{managerId}");

            return await result.ProcessResponseAsync<IEnumerable<CollaboratorResponse>>();
        }

        public async Task<Response<IEnumerable<CollaboratorResponse>>> GetCollaboratorsByDepartment(int departmentId)
        {
            var result = await _httpClient.GetAsync($"/api/v1/collaborators/department/{departmentId}");

            return await result.ProcessResponseAsync<IEnumerable<CollaboratorResponse>>();
        }

        public async Task<PagedResponse<IEnumerable<CollaboratorResponse>>> GetAllAsync(PagedRequest request)
        {
            var url = $"/api/v1/collaborators?pageNumber={request.PageNumber}&pageSize={request.PageSize}";

            if(!string.IsNullOrEmpty(request.SearchTerm))
                url += $"&searchTerm={request.SearchTerm}";

            if(!string.IsNullOrEmpty(request.FilterBy))
                url += $"&filterBy={request.FilterBy}";

            var result = await _httpClient.GetAsync(url);
            return await result.ProcessPagedResponseAsync<IEnumerable<CollaboratorResponse>>();
        }
    }
}