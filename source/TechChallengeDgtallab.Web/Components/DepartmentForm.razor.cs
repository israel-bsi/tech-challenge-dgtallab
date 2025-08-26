using Microsoft.AspNetCore.Components;
using MudBlazor;
using TechChallengeDgtallab.Core.Extensions;
using TechChallengeDgtallab.Core.Handler;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.Web.Components
{
    public partial class DepartmentFormComponent : ComponentBase
    {
        [Parameter] public int Id { get; set; }
        public EditDepartmentRequest InputModel { get; set; } = new();
        public IEnumerable<EditCollaboratorRequest> Collaborators { get; set; } = [];
        public IEnumerable<SuperiorDepartmentRequest> Departments { get; set; } = [];
        public bool IsBusy { get; set; }

        public string Operation => Id != 0
            ? "Editar"
            : "Cadastrar";

        [Inject] public IDepartmentHandler DepartmentHandler { get; set; } = null!;

        [Inject] public ICollaboratorHandler CollaboratorHandler { get; set; } = null!;

        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        public async Task OnValidSubmitAsync()
        {
            IsBusy = true;
            try
            {
                Response<DepartmentResponse> result;
                if (InputModel.Id > 0)
                    result = await DepartmentHandler.UpdateAsync(InputModel);
                else
                    result = await DepartmentHandler.AddAsync(InputModel);

                var resultMessage = result.Message ?? string.Empty;
                if (result.IsSuccess)
                {
                    Snackbar.Add(resultMessage, Severity.Success);
                    NavigationManager.NavigateTo("/departamentos");
                }
                else
                    Snackbar.Add(resultMessage, Severity.Error);
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            IsBusy = true;
            try
            {
                if (Id != 0)
                {
                    var result = await DepartmentHandler.GetByIdAsync(Id);
                    if (result is { IsSuccess: true, Data: not null })
                    {
                        InputModel.Id = result.Data.Id;
                        InputModel.Name = result.Data.Name;
                        InputModel.ManagerId = result.Data.Manager?.Id;
                        InputModel.Manager = new EditCollaboratorRequest
                        {
                            Id = result.Data.Manager?.Id ?? 0,
                            Name = result.Data.Manager?.Name ?? string.Empty
                        };
                        InputModel.SuperiorDepartmentId = result.Data.SuperiorDepartment?.Id;
                        InputModel.SuperiorDepartment = new SuperiorDepartmentRequest
                        {
                            Id = result.Data.SuperiorDepartment?.Id ?? 0,
                            Name = result.Data.SuperiorDepartment?.Name ?? string.Empty
                        };
                        await GetAllCollaboratorsByDepartmentAsync();
                        await GetAllDepartmentsAsync();
                    }
                    else
                    {
                        Snackbar.Add(result.Message ?? "Erro ao obter departamento", Severity.Error);
                        NavigationManager.NavigateTo("/departamentos");
                    }
                }
                else
                {
                    NavigationManager.NavigateTo("/departamentos/adicionar");
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task GetAllCollaboratorsByDepartmentAsync()
        {
            IsBusy = true;
            try
            {
                var result = await CollaboratorHandler.GetCollaboratorsByDepartment(InputModel.Id);
                if (result.IsSuccess)
                    Collaborators = result.Data?.ToRequest() ?? [];
                else
                    Snackbar.Add(result.Message ?? "Erro ao obter colaboradores", Severity.Error);
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task GetAllDepartmentsAsync()
        {
            IsBusy = true;
            try
            {
                var request = new PagedRequest { PageNumber = 1, PageSize = 1000 };
                var result = await DepartmentHandler.GetAllAsync(request);
                if (result.IsSuccess)
                    Departments = result
                        .Data?
                        .ToSuperiorRequest()
                        .Where(d=>d.Id != InputModel.Id) ?? [];
                else
                    Snackbar.Add(result.Message ?? "Erro ao obter departamentos", Severity.Error);
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnSelectedManagerValueChanged(EditCollaboratorRequest newValue)
        {
            InputModel.ManagerId = newValue.Id;
            InputModel.Manager = newValue;
        }

        public void OnSelectedSuperiorDepartmentValueChanged(SuperiorDepartmentRequest newValue)
        {
            InputModel.SuperiorDepartmentId = newValue.Id;
            InputModel.SuperiorDepartment = newValue;
        }
    }
}