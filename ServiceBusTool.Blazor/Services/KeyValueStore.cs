using ServiceBusTool.ServiceBus.Contract.Interfaces;

namespace ServiceBusTool.Blazor.Services;

public class KeyValueStore : IKeyValueStore
{
    public async Task<string?> GetValue(string key)
    {
        return await SecureStorage.Default.GetAsync(key);
    }

    public async Task SetValue(string key, string value)
    {
        await SecureStorage.Default.SetAsync(key, value);
    }
}