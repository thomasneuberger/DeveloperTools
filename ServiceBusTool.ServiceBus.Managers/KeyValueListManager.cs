using ServiceBusTool.ServiceBus.Contract.Interfaces;
using ServiceBusTool.ServiceBus.Models;
using System.Text.Json;

namespace ServiceBusTool.ServiceBus.Managers;

public abstract class KeyValueListManager<TValue> : IKeyValueListManager<TValue>
    where TValue : BaseKeyValueListItem
{
    protected abstract string StorageKey { get; }

    private readonly IKeyValueStore _keyValueStore;

    protected KeyValueListManager(IKeyValueStore keyValueStore)
    {
        _keyValueStore = keyValueStore;
    }

    public async Task<IEnumerable<TValue>> GetValuesAsync()
    {
        var storedValue = await _keyValueStore.GetValue(StorageKey) ?? "[]";
        return JsonSerializer.Deserialize<TValue[]>(storedValue)!;
    }

    public async Task<TValue> SaveValueAsync(TValue value)
    {
        var existingValues = (await GetValuesAsync()).ToList();
        if (existingValues.Any(c => c.Id == value.Id))
        {
            var existingValue = existingValues.First(c => c.Id == value.Id);
            existingValues.Remove(existingValue);
        }
        else
        {
            value.Id = Guid.NewGuid();
        }
        existingValues.Add(value);

        var storedValue = JsonSerializer.Serialize(existingValues);
        await _keyValueStore.SetValue(StorageKey, storedValue);
        return value;
    }

    public async Task DeleteValueAsync(Guid id)
    {
        var values = await GetValuesAsync();
        values = values
            .Where(c => c.Id != id)
            .ToList();
        var storedValue = JsonSerializer.Serialize(values);
        await _keyValueStore.SetValue(StorageKey, storedValue);
    }
}