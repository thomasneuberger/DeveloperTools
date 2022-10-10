using ServiceBusTool.ServiceBus.Models;

namespace ServiceBusTool.ServiceBus.Contract.Interfaces;

public interface IKeyValueListManager<TValue>
    where TValue : BaseKeyValueListItem
{
    Task<IEnumerable<TValue>> GetValuesAsync();

    Task<TValue> SaveValueAsync(TValue value);

    Task DeleteValueAsync(Guid id);
}