using Microsoft.AspNetCore.Components;
using MudBlazor;
using ServiceBusTool.ServiceBus.ViewModels;

namespace ServiceBusTool.Blazor.Pages;

public partial class Parameters
{
    [Inject]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private ParametersViewModel Model { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }
#pragma warning restore CS8618

    protected override async Task OnInitializedAsync()
    {
        await Model.OnInitializedAsync();
    }

    private async Task OnDelete()
    {
        if (Model.SelectedParameterDefinition is null)
        {
            await DialogService.ShowMessageBox("Delete", "Parameter definition deleted, but SelectedParameterDefinition is null.");

            return;
        }
            
        var delete = await DialogService.ShowMessageBox("Delete message definition", $"Delete message definition {Model.SelectedParameterDefinition.Name}?", "Delete", cancelText: "Cancel");

        if (delete == true)
        {
            await Model.OnDelete();
        }
    }
}