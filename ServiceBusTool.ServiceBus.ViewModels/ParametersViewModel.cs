using Microsoft.Extensions.Logging;
using ServiceBusTool.ServiceBus.Contract.Interfaces;
using ServiceBusTool.ServiceBus.Models;
using System.Data;

namespace ServiceBusTool.ServiceBus.ViewModels;

public class ParametersViewModel
{
    private readonly IKeyValueListManager<ParameterDefinition> _parameterDefinitionManager;
    private readonly ILogger<ParametersViewModel> _logger;

    private readonly List<ParameterDefinition> _parameterDefinitions = new();

    public ParametersViewModel(ILogger<ParametersViewModel> logger, IKeyValueListManager<ParameterDefinition> parameterDefinitionManager)
    {
        _logger = logger;
        _parameterDefinitionManager = parameterDefinitionManager;
    }

    public IReadOnlyList<ParameterDefinition> ParameterDefinitions => _parameterDefinitions;

    public ParameterDefinition? SelectedParameterDefinition { get; private set; }

    public async Task OnInitializedAsync()
    {
        await LoadParameterDefinitions();
    }

    private async Task LoadParameterDefinitions()
    {
        _parameterDefinitions.Clear();
        SelectedParameterDefinition = null;
        var parameterDefinitions = (await _parameterDefinitionManager
            .GetValuesAsync())
            .OrderBy(p => p.Name)
            .ToList();
        _parameterDefinitions.AddRange(parameterDefinitions);
    }

    public void SelectParameterDefinition(ParameterDefinition parameterDefinition)
    {
        SelectedParameterDefinition = parameterDefinition;
    }

    public void AddParameterDefinition()
    {
        SelectedParameterDefinition = new ParameterDefinition();
    }

    public void AddParameter()
    {
        if (SelectedParameterDefinition is null)
        {
            _logger.LogError("Parameter added, but SelectedParameterDefinition is null.");
            return;
        }

        SelectedParameterDefinition.Parameters.Add(new Parameter());
    }

    public void DeleteParameter(int index)
    {
        if (SelectedParameterDefinition is null)
        {
            _logger.LogError("Parameter deleted, but SelectedParameterDefinition is null.");
            return;
        }

        SelectedParameterDefinition.Parameters.RemoveAt(index);
    }

    public async Task OnSave()
    {
        if (SelectedParameterDefinition is null)
        {
            _logger.LogError("Parameter definition saved, but SelectedParameterDefinition is null.");
            return;
        }

        await _parameterDefinitionManager.SaveValueAsync(SelectedParameterDefinition);
        await LoadParameterDefinitions();
    }

    public async Task OnDelete()
    {
        if (SelectedParameterDefinition is null)
        {
            _logger.LogError("Parameter definition deleted, but SelectedParameterDefinition is null.");
            return;
        }

        await _parameterDefinitionManager.DeleteValueAsync(SelectedParameterDefinition.Id);
        await LoadParameterDefinitions();
    }
}