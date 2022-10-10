using ServiceBusTool.ServiceBus.Contract.Interfaces;
using ServiceBusTool.ServiceBus.Models;

namespace ServiceBusTool.ServiceBus.Managers;

public class ConnectionManager : KeyValueListManager<Connection>
{
    protected override string StorageKey => "Connections";

    public ConnectionManager(IKeyValueStore keyValueStore)
        :base(keyValueStore)
    {
    }
}