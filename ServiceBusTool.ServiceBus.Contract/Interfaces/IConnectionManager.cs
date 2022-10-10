using ServiceBusTool.ServiceBus.Models;

namespace ServiceBusTool.ServiceBus.Contract.Interfaces;

public interface IConnectionManager
{
    Task<IEnumerable<Connection>> GetConnectionsAsync();

    Task<Connection> SaveConnection(Connection connection);

    Task DeleteConnection(Guid id);
}