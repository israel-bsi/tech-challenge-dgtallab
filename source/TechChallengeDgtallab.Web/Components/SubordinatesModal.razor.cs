using Microsoft.AspNetCore.Components;
using MudBlazor;
using TechChallengeDgtallab.Core.DTOs;
using TechChallengeDgtallab.Core.Extensions;
using TechChallengeDgtallab.Core.Handler;

namespace TechChallengeDgtallab.Web.Components;

public class SubordinatesModalBase : ComponentBase
{
    [Parameter] public int ManagerId { get; set; }
    [Parameter] public string ManagerName { get; set; } = string.Empty;
    
    [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = null!;
    
    [Inject] public ICollaboratorHandler Handler { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    protected bool IsLoading { get; set; } = true;
    protected IEnumerable<CollaboratorDto>? Subordinates { get; set; }
    protected string ErrorMessage { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await LoadSubordinatesAsync();
    }

    private async Task LoadSubordinatesAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            
            var response = await Handler.GetSubordinatesAsync(ManagerId);
            
            if (response.IsSuccess && response.Data != null)
            {
                Subordinates = response.Data.Select(s => s.ToDto());
            }
            else
            {
                ErrorMessage = response.Message ?? "Erro ao carregar subordinados.";
                if (response.StatusCode != 200)
                {
                    Snackbar.Add(ErrorMessage, Severity.Warning);
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Erro interno ao carregar subordinados.";
            Snackbar.Add(ErrorMessage, Severity.Error);
        }
        finally
        {
            IsLoading = false;
            StateHasChanged();
        }
    }

    protected void Close() => MudDialog.Close();
}