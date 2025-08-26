using Microsoft.AspNetCore.Components;
using MudBlazor;
using TechChallengeDgtallab.Core.Handler;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Responses;

namespace TechChallengeDgtallab.Web.Components
{
    public partial class DepartmentFormComponent : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }
        public EditDepartmentRequest InputModel { get; set; } = new();
        public bool IsBusy { get; set; }

        [Inject] public IDepartmentHandler Handler { get; set; } = null!;

        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        public async Task OnValidSubmitAsync()
        {
            IsBusy = true;
            try
            {
                Response<DepartmentResponse> result;
                if (InputModel.Id > 0)
                    result = await Handler.UpdateAsync(InputModel);
                else
                    result = await Handler.AddAsync(InputModel);

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
                    var result = await Handler.GetByIdAsync(Id);
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
                        InputModel.SuperiorDepartment = new EditDepartmentRequest
                        {
                            Id = result.Data.SuperiorDepartment?.Id ?? 0,
                            Name = result.Data.SuperiorDepartment?.Name ?? string.Empty
                        };
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
    }
}