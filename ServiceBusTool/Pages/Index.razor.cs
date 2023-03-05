using Microsoft.AspNetCore.Components;
using MudBlazor;
using ServiceBusTool.ServiceBus.Models;
using ServiceBusTool.ServiceBus.ViewModels;

namespace ServiceBusTool.Pages;

public partial class Index
{
    [Inject]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private IndexViewModel Model { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }
#pragma warning restore CS8618

    protected override async Task OnInitializedAsync()
    {
        Model.QueueNamesLoaded += QueueNamesLoaded;
        await Model.OnInitializedAsync();
    }
    

    private void QueueNamesLoaded(object? sender, EventArgs e)
    {
        StateHasChanged();
    }

    public Connection? SelectedConnection
    {
        get => Model.SelectedConnection;
        set => Model.SelectConnection(value);
    }

    public string? SelectedQueueName
    {
        get => Model.SelectedQueueName;
        set => Model.SelectQueueName(value);
    }

    public string? Message
    {
        get => Model.SelectedMessage;
        set
        {
            Model.SelectedMessage = value ?? string.Empty;
            Model.OnMessageChanged(value);
        }
    }
}