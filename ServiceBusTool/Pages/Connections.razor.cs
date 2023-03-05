using Microsoft.AspNetCore.Components;
using MudBlazor;
using ServiceBusTool.ServiceBus.ViewModels;

namespace ServiceBusTool.Pages
{
    public partial class Connections
    {
        [Inject]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private ConnectionsViewModel Model { get; set; }

        [Inject]
        private IDialogService DialogService { get; set; }
#pragma warning restore CS8618

        protected override async Task OnInitializedAsync()
        {
            await Model.LoadConnections();
        }

        private async Task OnDelete()
        {
            if (Model.SelectedConnection is null)
            {
                await DialogService.ShowMessageBox("Delete", "Connection deleted, but selectedConnection is null.");

                return;
            }
            
            var delete = await DialogService.ShowMessageBox("Delete connection", $"Delete connection {Model.SelectedConnection.Name}?", "Delete", cancelText: "Cancel");

            if (delete == true)
            {
                await Model.OnDelete();
            }
        }
    }
}
