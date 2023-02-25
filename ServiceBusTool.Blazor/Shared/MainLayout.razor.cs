namespace ServiceBusTool.Blazor.Shared;

public partial class MainLayout
{
    private bool _drawerOpen;

    private void ToggleDrawer()
    {
        _drawerOpen = !_drawerOpen;
    }
}