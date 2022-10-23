using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using ServiceBusTool.ServiceBus.Contract.Interfaces;
using ServiceBusTool.ServiceBus.Models;

namespace ServiceBusTool.ServiceBus.Azure;

public class AzureServiceBus : IServiceBus
{
    private readonly ILogger<AzureServiceBus> _logger;

    public AzureServiceBus(ILogger<AzureServiceBus> logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<string>> GetQueueNames(Connection connection, CancellationToken cancellationToken)
    {
        var client = new ServiceBusAdministrationClient(connection.ConnectionString);
        
        var queues = await client.GetQueuesAsync(cancellationToken)
            .ToListAsync(cancellationToken);
        var queueNames = queues.Select(q => q.Name).ToList();
        
        return queueNames;
    }

    public async Task SendMessage(
        Connection connection,
        string queueName,
        string message,
        IEnumerable<Parameter> parameters,
        CancellationToken cancellationToken)
    {
        try
        {
            await using var client = new ServiceBusClient(connection.ConnectionString);

            await using var sender = client.CreateSender(queueName);

            var completeMessage = message;
            foreach (var parameter in parameters)
            {
                completeMessage = completeMessage.Replace(
                    $"%{parameter.Name}%",
                    parameter.Value,
                    StringComparison.InvariantCultureIgnoreCase);
            }

            await sender.SendMessageAsync(new ServiceBusMessage(completeMessage), cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception sending message {Message}: {Exception}", message, ex);
        }
    }
}