using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using ServiceBusTool.ServiceBus.ViewModels;

namespace ServiceBusTool.Blazor.Pages
{
    public class ConnectionsBase : ComponentBase
    {
        protected bool ShowConfirmationDelete { get; set; }

        [Inject]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected ConnectionsViewModel Model { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        protected override async Task OnInitializedAsync()
        {
            await Model.LoadConnections();
        }

        protected void OnDelete()
        {
            if (Model.SelectedConnection is null)
            {
                Application.Current?.MainPage?.DisplayAlert(
                    "Delete",
                    "Connection deleted, but selectedConnection is null.",
                    "Ok");

                return;
            }

            ShowConfirmationDelete = true;
        }

        protected async Task OnDeleteConfirmed()
        {
            await Model.OnDelete();

            ShowConfirmationDelete = false;
        }
    }
}
