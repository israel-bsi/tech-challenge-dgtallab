using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using TechChallengeDgtallab.Core.DTOs;
using TechChallengeDgtallab.Core.Extensions;
using TechChallengeDgtallab.Core.Handler;
using TechChallengeDgtallab.Core.Requests;
using TechChallengeDgtallab.Web.Components;

namespace TechChallengeDgtallab.Web.Pages.Departments
{
    public partial class ListDepartmentPage : ComponentBase
    {
        public MudDataGrid<DepartmentDto> DataGrid { get; set; } = null!;

        public string SearchTerm { get; set; } = string.Empty;
        public FilterOption SelectedFilter { get; set; } = new();

        public readonly List<FilterOption> FilterOptions =
        [
            new() { DisplayName = "Nome", PropertyName = "Name" },
            new() { DisplayName = "Gerente", PropertyName = "Manager.Name" },
            new() { DisplayName = "Departamento Superior", PropertyName = "SuperiorDepartment.Name" }
        ];

        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        [Inject] public IDialogService DialogService { get; set; } = null!;

        [Inject] public IDepartmentHandler Handler { get; set; } = null!;

        public async Task<GridData<DepartmentDto>> LoadServerData(GridState<DepartmentDto> state)
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
                    return new GridData<DepartmentDto>
                    {
                        Items = response.Data?.Select(d => d.ToDto()) ?? [],
                        TotalItems = response.TotalCount
                    };
                }

                Snackbar.Add(response.Message ?? string.Empty, Severity.Error);
                return new GridData<DepartmentDto>
                {
                    Items = [],
                    TotalItems = 0
                };
            }
            catch (Exception e)
            {
                Snackbar.Add(e.Message, Severity.Error);
                return new GridData<DepartmentDto>
                {
                    Items = [],
                    TotalItems = 0
                };
            }
        }

        public async Task ShowHierarchyModalAsync(int departmentId, string departmentName)
        {
            var parameters = new DialogParameters
            {
                { "DepartmentId", departmentId },
                { "DepartmentName", departmentName }
            };

            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.ExtraLarge,
                FullWidth = true,
                CloseOnEscapeKey = true
            };

            await DialogService.ShowAsync<DepartmentHierarchyModal>(
                "Hierarquia de Departamentos", 
                parameters, 
                options);
        }

        public async Task OnDeleteButtonClickedAsync(int id, string name)
        {
            var parameters = new DialogParameters
            {
                { "ContentText", $"Ao prosseguir o departamento {name} será excluido. " +
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
                Snackbar.Add($"Departamento {name} excluído", Severity.Success);
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
}