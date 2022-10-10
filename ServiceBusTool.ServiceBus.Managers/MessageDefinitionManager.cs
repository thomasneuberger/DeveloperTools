using System.Text.RegularExpressions;
using ServiceBusTool.ServiceBus.Contract.Interfaces;
using ServiceBusTool.ServiceBus.Models;
using System.Linq;

namespace ServiceBusTool.ServiceBus.Managers;

public class MessageDefinitionManager : KeyValueListManager<MessageDefinition>
{
    protected override string StorageKey => "MessageDefinitions";

    public MessageDefinitionManager(IKeyValueStore keyValueStore)
        :base(keyValueStore)
    {
    }
}