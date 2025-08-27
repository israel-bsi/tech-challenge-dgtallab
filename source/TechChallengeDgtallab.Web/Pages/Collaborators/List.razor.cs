using Microsoft.AspNetCore.Components;
using MudBlazor;
using TechChallengeDgtallab.Core.DTOs;
using TechChallengeDgtallab.Core.Extensions;
using TechChallengeDgtallab.Core.Handler;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Web.Components;

namespace TechChallengeDgtallab.Web.Pages.Collaborators;

public class ListCollaboratorPage : ComponentBase
{
    public MudDataGrid<CollaboratorDto> DataGrid { get; set; } = null!;

    public string SearchTerm { get; set; } = string.Empty;
    public FilterOption SelectedFilter { get; set; } = new();

    public readonly List<FilterOption> FilterOptions =
    [
        new() { DisplayName = "Nome", PropertyName = "Name" },
        new() { DisplayName = "CPF", PropertyName = "Cpf" },
        new() { DisplayName = "RG", PropertyName = "Rg" },
        new() { DisplayName = "Departamento", PropertyName = "Department.Name" }
    ];

    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    [Inject] public IDialogService DialogService { get; set; } = null!;

    [Inject] public ICollaboratorHandler Handler { get; set; } = null!;

    public async Task<GridData<CollaboratorDto>> LoadServerData(GridState<CollaboratorDto> state)
    {
        try
        {
            var request = new PagedRequest
            {
                PageNumber = state.Page + 1,
                PageSize = state.PageSize,
                SearchTerm = SearchTerm,
                FilterBy = SelectedFilter.PropertyName
            };

            var response = await Handler.GetAllAsync(request);
            if (response.IsSuccess)
            {
                return new GridData<CollaboratorDto>
                {
                    Items = response.Data?.Select(c => c.ToDto()) ?? [],
                    TotalItems = response.TotalCount
                };
            }

            Snackbar.Add(response.Message ?? string.Empty, Severity.Error);
            return new GridData<CollaboratorDto>
            {
                Items = [],
                TotalItems = 0
            };
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
            return new GridData<CollaboratorDto>
            {
                Items = [],
                TotalItems = 0
            };
        }
    }

    public async Task OnDeleteButtonClickedAsync(int id, string name)
    {
        var parameters = new DialogParameters
            {
                { "ContentText", $"Ao prosseguir o colaborador {name} será excluido. " +
                                 "Deseja continuar?" },
                { "ButtonText", "Confirmar" },
                { "ButtonColor", Color.Error }
            };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small
        };

        var dialog = await DialogService.ShowAsync<DialogConfirm>("Atenção", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: true }) return;

        await OnDeleteAsync(id, name);
        StateHasChanged();
    }

    private async Task OnDeleteAsync(int id, string name)
    {
        try
        {
            await Handler.DeleteAsync(id);
            await DataGrid.ReloadServerData();
            Snackbar.Add($"Colaborador {name} excluído", Severity.Success);
        }
        catch (Exception e)
        {
            Snackbar.Add(e.Message, Severity.Error);
        }
    }

    public void OnValueFilterChanged(FilterOption newValue)
    {
        SelectedFilter = newValue;
    }
    public void OnButtonSearchClick() => DataGrid.ReloadServerData();

    public void OnClearSearchClick()
    {
        SearchTerm = string.Empty;
        SelectedFilter = new FilterOption();
        DataGrid.ReloadServerData();
    }
}