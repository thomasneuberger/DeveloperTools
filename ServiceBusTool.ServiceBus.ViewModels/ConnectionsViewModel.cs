using Microsoft.Extensions.Logging;
using ServiceBusTool.ServiceBus.Contract.Interfaces;
using ServiceBusTool.ServiceBus.Models;
using System.Text.Json;

namespace ServiceBusTool.ServiceBus.ViewModels;

public class ConnectionsViewModel
{
    private readonly IKeyValueListManager<Connection> _connectionManager;
    private readonly ILogger<ConnectionsViewModel> _logger;
    private readonly List<Connection> _connections = new();

    public ConnectionsViewModel(IKeyValueListManager<Connection> connectionManager, ILogger<ConnectionsViewModel> logger)
    {
        _connectionManager = connectionManager;
        _logger = logger;
    }

    public IReadOnlyList<Connection> Connections => _connections;

    public Connection? SelectedConnection { get; private set; }

    public void OnConnectionSelected(Connection connection)
    {
        SelectedConnection = JsonSerializer.Deserialize<Connection>(JsonSerializer.Serialize(connection));
    }

    public async Task LoadConnections()
    {
        SelectedConnection = null;
        _connections.Clear();
        var connections = (await _connectionManager
                .GetValuesAsync())
            .OrderBy(c => c.Name)
            .ToList();
        _connections.AddRange(connections);
    }

    public void OnAddConnection()
    {
        SelectedConnection = new Connection();
    }

    public async Task OnSubmit()
    {
        if (SelectedConnection is null)
        {
            _logger.LogError("Connection saved, but selectedConnection is null.");
            return;
        }
        await _connectionManager.SaveValueAsync(SelectedConnection);
        await LoadConnections();
    }

    public async Task OnDelete()
    {
        if (SelectedConnection is null)
        {
            _logger.LogError("Connection deleted, but selectedConnection is null.");
            return;
        }

        await _connectionManager.DeleteValueAsync(SelectedConnection.Id);
        await LoadConnections();
    }
}