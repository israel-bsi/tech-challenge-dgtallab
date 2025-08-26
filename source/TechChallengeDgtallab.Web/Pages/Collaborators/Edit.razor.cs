using Microsoft.AspNetCore.Components;
using MudBlazor;
using TechChallengeDgtallab.Core.Extensions;
using TechChallengeDgtallab.Core.Handler;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Requests.Collaborator;
using TechChallengeDgtallab.Core.Requests.Department;

namespace TechChallengeDgtallab.Web.Pages.Collaborators;

public partial class EditCollaboratorPage : ComponentBase
{
    [Parameter] public int Id { get; set; }
    public UpdateCollaboratorRequest InputModel { get; set; } = new();
    public IEnumerable<DepartmentDto> Departments { get; set; } = [];
    public bool IsBusy { get; set; }

    [Inject] public IDepartmentHandler DepartmentHandler { get; set; } = null!;

    [Inject] public ICollaboratorHandler CollaboratorHandler { get; set; } = null!;

    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;
        try
        {
            var result = await CollaboratorHandler.UpdateAsync(InputModel);

            var resultMessage = result.Message ?? string.Empty;
            if (result.IsSuccess)
            {
                Snackbar.Add(resultMessage, Severity.Success);
                NavigationManager.NavigateTo("/colaboradores");
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
            var result = await CollaboratorHandler.GetByIdAsync(Id);
            if (result is { IsSuccess: true, Data: not null })
            {
                InputModel.Id = result.Data.Id;
                InputModel.Name = result.Data.Name;
                InputModel.Cpf = result.Data.Cpf;
                InputModel.Rg = result.Data.Rg;
                InputModel.Department = new DepartmentDto
                {
                    Id = result.Data.Department?.Id ?? 0,
                    Name = result.Data.Department?.Name ?? string.Empty
                };
                InputModel.DepartmentId = result.Data.Department?.Id ?? 0;
                await GetAllDepartmentsAsync();
            }
            else
                Snackbar.Add(result.Message ?? "Erro ao obter colaborador", Severity.Error);
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

    private async Task GetAllDepartmentsAsync()
    {
        IsBusy = true;
        try
        {
            var request = new PagedRequest { PageNumber = 1, PageSize = 1000 };
            var result = await DepartmentHandler.GetAllAsync(request);
            if (result.IsSuccess)
                Departments = result
                    .Data?
                    .ToDto() ?? [];
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

    public void OnSelectedDepartmentValueChanged(DepartmentDto dto)
    {
        InputModel.DepartmentId = dto.Id;
        InputModel.Department = dto;
    }
}