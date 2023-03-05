using Microsoft.AspNetCore.Components;
using MudBlazor;
using ServiceBusTool.ServiceBus.ViewModels;

namespace ServiceBusTool.Pages;

public partial class Messages
{
    [Inject]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private MessagesViewModel Model { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }
#pragma warning restore CS8618

    protected override async Task OnInitializedAsync()
    {
        await Model.LoadMessageDefinitions();
    }

    public string Body
    {
        get => Model.SelectedMessageDefinition?.Body ?? string.Empty;
        set => Model.OnBodyChanged(value);
    }

    private async Task OnDelete()
    {
        if (Model.SelectedMessageDefinition is null)
        {
            await DialogService.ShowMessageBox("Delete", "Message definition deleted, but SelectedMessageDefinition is null.");

            return;
        }
            
        var delete = await DialogService.ShowMessageBox("Delete message definition", $"Delete message definition {Model.SelectedMessageDefinition.Name}?", "Delete", cancelText: "Cancel");

        if (delete == true)
        {
            await Model.OnDelete();
        }
    }
}