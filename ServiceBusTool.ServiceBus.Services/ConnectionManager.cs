using System.Text.Json;
using ServiceBusTool.ServiceBus.Contract.Interfaces;
using ServiceBusTool.ServiceBus.Models;

namespace ServiceBusTool.ServiceBus.Services;

public class ConnectionManager : IConnectionManager
{
    private const string StorageKey = "Connections";

    private readonly IKeyValueStore _keyValueStore;
    private readonly IList<Connection> _connections = new List<Connection>
    {
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Development",
            ConnectionString = "Dev-ConnectionString"
        },
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Production",
            ConnectionString = "Prod-ConnectionString"
        }
    };

    public ConnectionManager(IKeyValueStore keyValueStore)
    {
        _keyValueStore = keyValueStore;
    }

    public async Task<IEnumerable<Connection>> GetConnectionsAsync()
    {
        var storedValue = await _keyValueStore.GetValue(StorageKey) ?? "[]";
        return JsonSerializer.Deserialize<Connection[]>(storedValue)!;
    }

    public async Task<Connection> SaveConnection(Connection connection)
    {
        var existingConnections = (await GetConnectionsAsync()).ToList();
        if (existingConnections.Any(c => c.Id == connection.Id))
        {
            var existingConnection = existingConnections.First(c => c.Id == connection.Id);
            existingConnection.Name = connection.Name;
            existingConnection.ConnectionString = connection.ConnectionString;
        }
        else
        {
            connection.Id = Guid.NewGuid();
            existingConnections.Add(connection);
        }

        var storedValue = JsonSerializer.Serialize(existingConnections);
        await _keyValueStore.SetValue(StorageKey, storedValue);
        return connection;
    }

    public async Task DeleteConnection(Guid id)
    {
        var connections = await GetConnectionsAsync();
        connections = connections
            .Where(c => c.Id != id)
            .ToList();
        var storedValue = JsonSerializer.Serialize(connections);
        await _keyValueStore.SetValue(StorageKey, storedValue);
    }
}