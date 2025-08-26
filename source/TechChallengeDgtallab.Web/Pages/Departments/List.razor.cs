using Microsoft.AspNetCore.Components;
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

        private string _searchTerm = string.Empty;

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                if (_searchTerm == value) return;
                _searchTerm = value;
                _ = DataGrid.ReloadServerData();
            }
        }

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
                    SearchTerm = SearchTerm
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
    }
}