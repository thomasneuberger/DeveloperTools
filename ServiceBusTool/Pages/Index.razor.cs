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
    private ISnackbar Snackbar { get; set; }
#pragma warning restore CS8618

    protected override async Task OnInitializedAsync()
    {
        Model.QueueNamesLoaded += QueueNamesLoaded;
        Model.MessageSent += MessageSent;
        Model.MessageError += MessageError;
        await Model.OnInitializedAsync();
    }

    private void QueueNamesLoaded(object? sender, EventArgs e)
    {
        StateHasChanged();
    }

    private void MessageSent(object? sender, string message)
    {
        Snackbar.Add("Message sent", Severity.Success);
    }

    private void MessageError(object? sender, Exception exception)
    {
        Snackbar.Add($"Error sending message: {exception.Message}", Severity.Error, options =>
        {
            options.RequireInteraction = true;
        });
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