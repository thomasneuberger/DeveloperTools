namespace ServiceBusTool.ServiceBus.Contract.Interfaces;

public interface IKeyValueStore
{
    Task<string?> GetValue(string key);

    Task SetValue(string key, string value);
}