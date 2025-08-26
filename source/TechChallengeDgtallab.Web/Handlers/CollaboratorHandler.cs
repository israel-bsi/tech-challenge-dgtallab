using System.Net.Http.Json;
using TechChallengeDgtallab.Core.Handler;
using TechChallengeDgtallab.Core.Requests;
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
        public async Task<Response<CollaboratorResponse>> AddAsync(EditCollaboratorRequest request)
        {
            var result = await _httpClient.PostAsJsonAsync("/api/v1/collaborators", request);

            return await result.Content.ReadFromJsonAsync<Response<CollaboratorResponse>>()
                   ?? new Response<CollaboratorResponse>(null, 400, "Erro ao criar o colaborador");
        }

        public async Task<Response<CollaboratorResponse>> UpdateAsync(EditCollaboratorRequest request)
        {
            var result = await _httpClient.PutAsJsonAsync("/api/v1/collaborators", request);

            return await result.Content.ReadFromJsonAsync<Response<CollaboratorResponse>>()
                   ?? new Response<CollaboratorResponse>(null, 400, "Erro ao atualizar o colaborador");
        }

        public async Task<Response<CollaboratorResponse>> DeleteAsync(int id)
        {
            var result = await _httpClient.DeleteAsync($"/api/v1/collaborators/{id}");

            return await result.Content.ReadFromJsonAsync<Response<CollaboratorResponse>>()
                   ?? new Response<CollaboratorResponse>(null, 400, "Erro ao deletar o colaborador");
        }

        public async Task<Response<CollaboratorResponse>> GetByIdAsync(int id)
        {
            var result = await _httpClient.GetAsync($"/api/v1/collaborators/{id}");

            return await result.Content.ReadFromJsonAsync<Response<CollaboratorResponse>>()
                   ?? new Response<CollaboratorResponse>(null, 400, "Erro ao obter o colaborador");
        }

        public async Task<Response<IEnumerable<CollaboratorResponse>>> GetSubordinatesAsync(int managerId)
        {
            var result = await _httpClient.GetAsync($"/api/v1/collaborators/subordinates/{managerId}");

            return await result.Content.ReadFromJsonAsync<Response<IEnumerable<CollaboratorResponse>>>()
                   ?? new Response<IEnumerable<CollaboratorResponse>>(null, 400, "Erro ao obter os subordinados");
        }

        public async Task<PagedResponse<IEnumerable<CollaboratorResponse>>> GetAllAsync(PagedRequest request)
        {
            var url = $"/api/v1/collaborators?pageNumber={request.PageNumber}&pageSize={request.PageSize}";

            return await _httpClient.GetFromJsonAsync<PagedResponse<IEnumerable<CollaboratorResponse>>>(url)
                   ?? new PagedResponse<IEnumerable<CollaboratorResponse>>(null, 400, "Erro ao obter os colaboradores");
        }
    }
}