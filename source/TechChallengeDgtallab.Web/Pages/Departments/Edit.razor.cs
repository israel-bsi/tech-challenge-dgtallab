using Microsoft.AspNetCore.Components;
using MudBlazor;
using TechChallengeDgtallab.Core.DTOs;
using TechChallengeDgtallab.Core.Extensions;
using TechChallengeDgtallab.Core.Handler;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Core.Requests.Department;

namespace TechChallengeDgtallab.Web.Pages.Departments;

public partial class EditDepartmentPage : ComponentBase
{
    [Parameter] public int Id { get; set; }
    public UpdateDepartmentRequest InputModel { get; set; } = new();
    public IEnumerable<CollaboratorDto> Collaborators { get; set; } = [];
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
            var result = await DepartmentHandler.UpdateAsync(InputModel);

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
            var result = await DepartmentHandler.GetByIdAsync(Id);
            if (result is { IsSuccess: true, Data: not null })
            {
                InputModel.Id = result.Data.Id;
                InputModel.Name = result.Data.Name;
                InputModel.ManagerId = result.Data.Manager?.Id;
                InputModel.Manager = new CollaboratorDto
                {
                    Id = result.Data.Manager?.Id ?? 0,
                    Cpf = result.Data.Manager?.Cpf ?? string.Empty,
                    Name = result.Data.Manager?.Name ?? string.Empty,
                    DepartmentId = Id
                };
                InputModel.SuperiorDepartmentId = result.Data.SuperiorDepartment?.Id;
                InputModel.SuperiorDepartment = new DepartmentDto
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
                Collaborators = result.Data?.Select(c => c.ToDto()) ?? [];
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
                Departments = result.Data?
                   .Select(d => d.ToDto())
                   .Where(d => d.Id != InputModel.Id) ?? [];
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

    public void OnSelectedManagerValueChanged(CollaboratorDto newValue)
    {
        InputModel.ManagerId = newValue.Id;
        InputModel.Manager = newValue;
    }

    public void OnSelectedSuperiorDepartmentValueChanged(DepartmentDto newValue)
    {
        InputModel.SuperiorDepartmentId = newValue.Id;
        InputModel.SuperiorDepartment = newValue;
    }
}