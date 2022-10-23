using ServiceBusTool.ServiceBus.Models;

namespace ServiceBusTool.ServiceBus.Contract.Interfaces;

public interface IServiceBus
{
    Task<IEnumerable<string>> GetQueueNames(Connection connection, CancellationToken cancellationToken);

    Task SendMessage(
        Connection connection,
        string queueName,
        string message,
        IEnumerable<Parameter> parameters,
        CancellationToken cancellationToken);
}
