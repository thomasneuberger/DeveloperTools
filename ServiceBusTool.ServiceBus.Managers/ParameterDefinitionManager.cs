using ServiceBusTool.ServiceBus.Contract.Interfaces;
using ServiceBusTool.ServiceBus.Models;

namespace ServiceBusTool.ServiceBus.Managers;

public class ParameterDefinitionManager : KeyValueListManager<ParameterDefinition>
{
    public ParameterDefinitionManager(IKeyValueStore keyValueStore)
        : base(keyValueStore)
    {
    }

    protected override string StorageKey => "ParameterDefinitions";
}