using Microsoft.AspNetCore.Components;
using MudBlazor;
using TechChallengeDgtallab.Core.DTOs;
using TechChallengeDgtallab.Core.Extensions;
using TechChallengeDgtallab.Core.Handler;

namespace TechChallengeDgtallab.Web.Components;

public class DepartmentHierarchyModalBase : ComponentBase
{
    [Parameter] public int DepartmentId { get; set; }
    [Parameter] public string DepartmentName { get; set; } = string.Empty;
    
    [CascadingParameter] IMudDialogInstance MudDialog { get; set; } = null!;
    
    [Inject] public IDepartmentHandler Handler { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    protected bool IsLoading { get; set; } = true;
    protected IEnumerable<DepartmentDto>? HierarchyDepartments { get; set; }
    protected string ErrorMessage { get; set; } = string.Empty;

    private Dictionary<int, int> _departmentLevels = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadHierarchyAsync();
    }

    private async Task LoadHierarchyAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            
            var response = await Handler.GetDepartmentHierarchyAsync(DepartmentId);
            
            if (response.IsSuccess && response.Data != null)
            {
                var departments = response.Data.Select(d => d.ToDto()).ToList();
                HierarchyDepartments = departments;
                
                CalculateDepartmentLevels(departments);
                
                // Debug: Log dos níveis calculados
                Console.WriteLine($"Departamentos carregados: {departments.Count}");
                foreach (var dept in departments)
                {
                    var level = _departmentLevels.TryGetValue(dept.Id, out var l) ? l : -1;
                    Console.WriteLine($"Dept ID: {dept.Id}, Nome: {dept.Name}, Superior: {dept.SuperiorDepartmentId}, Nível: {level}");
                }
            }
            else
            {
                ErrorMessage = response.Message ?? "Erro ao carregar hierarquia de departamentos.";
                if (response.StatusCode != 200)
                {
                    Snackbar.Add(ErrorMessage, Severity.Warning);
                }
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Erro interno ao carregar hierarquia de departamentos.";
            Snackbar.Add($"Erro: {ex.Message}", Severity.Error);
        }
        finally
        {
            IsLoading = false;
            StateHasChanged();
        }
    }

    private void CalculateDepartmentLevels(List<DepartmentDto> departments)
    {
        _departmentLevels.Clear();
        
        var rootDepartment = departments.FirstOrDefault(d => d.Id == DepartmentId);
        if (rootDepartment != null)
        {
            Console.WriteLine($"Departamento raiz encontrado: {rootDepartment.Name} (ID: {rootDepartment.Id})");
            CalculateLevelRecursive(rootDepartment, departments, 0);
        }
        else
        {
            Console.WriteLine($"Departamento raiz não encontrado! Buscando ID: {DepartmentId}");
            Console.WriteLine($"Departamentos disponíveis: {string.Join(", ", departments.Select(d => $"{d.Name}({d.Id})"))}");
        }
    }

    private void CalculateLevelRecursive(DepartmentDto department, List<DepartmentDto> allDepartments, int level)
    {
        _departmentLevels[department.Id] = level;
        Console.WriteLine($"Definindo nível {level} para departamento {department.Name} (ID: {department.Id})");
        
        var children = allDepartments
            .Where(d => d.SuperiorDepartmentId == department.Id && d.Id != department.Id)
            .ToList();

        Console.WriteLine($"Departamento {department.Name} tem {children.Count} filhos: {string.Join(", ", children.Select(c => c.Name))}");

        foreach (var child in children)
        {
            CalculateLevelRecursive(child, allDepartments, level + 1);
        }
    }

    protected string GetDepartmentIcon(DepartmentDto department)
    {
        if (department.Id == DepartmentId)
            return Icons.Material.Filled.Business;
        
        if (HierarchyDepartments?.Any(d => d.SuperiorDepartmentId == department.Id && d.Id != department.Id) == true)
            return Icons.Material.Filled.BusinessCenter;
        
        return Icons.Material.Filled.Apartment;
    }

    protected Color GetDepartmentIconColor(DepartmentDto department)
    {
        if (department.Id == DepartmentId)
            return Color.Primary;
        
        if (HierarchyDepartments?.Any(d => d.SuperiorDepartmentId == department.Id && d.Id != department.Id) == true)
            return Color.Success;
        
        return Color.Default;
    }

    protected string GetDepartmentLevel(DepartmentDto department)
    {
        var level = _departmentLevels.GetValueOrDefault(department.Id, -1);
        Console.WriteLine($"GetDepartmentLevel para {department.Name} (ID: {department.Id}): {level}");
        return level >= 0 ? level.ToString() : "N/A";
    }

    protected int GetMaxLevel()
    {
        var maxLevel = _departmentLevels.Values.DefaultIfEmpty(0).Max();
        Console.WriteLine($"Nível máximo calculado: {maxLevel}");
        return maxLevel;
    }

    protected void Close() => MudDialog.Close();
}