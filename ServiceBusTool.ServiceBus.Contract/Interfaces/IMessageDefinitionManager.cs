using ServiceBusTool.ServiceBus.Models;

namespace ServiceBusTool.ServiceBus.Contract.Interfaces;

public interface IMessageDefinitionManager : IKeyValueListManager<MessageDefinition>
{
    IReadOnlyList<string> FindParameters(MessageDefinition message);
}