using ServiceBusTool.ServiceBus.Contract.Interfaces;
using ServiceBusTool.ServiceBus.Models;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ServiceBusTool.ServiceBus.ViewModels;

public class IndexViewModel
{
    private readonly IKeyValueListManager<Connection> _connectionManager;
    private readonly IKeyValueListManager<MessageDefinition> _messageDefinitionManager;
    private readonly IServiceBus _serviceBus;
    private readonly ILogger<IndexViewModel> _logger;

    private readonly List<string> _queueNames = new();

    public IndexViewModel(
        IKeyValueListManager<Connection> connectionManager,
        IServiceBus serviceBus,
        ILogger<IndexViewModel> logger,
        IKeyValueListManager<MessageDefinition> messageDefinitionManager)
    {
        _connectionManager = connectionManager;
        _serviceBus = serviceBus;
        _logger = logger;
        _messageDefinitionManager = messageDefinitionManager;
    }

    public IEnumerable<Connection>? Connections { get; private set; }
    public Connection? SelectedConnection { get; private set; }
    public List<string> QueueNames => _queueNames;
    public string? SelectedQueueName { get; set; }

    public IEnumerable<MessageDefinition>? MessageDefinitions { get; private set; }

    public string SelectedMessage { get; set; } = string.Empty;
    public List<Parameter> MessageParameters { get; } = new();

    public event EventHandler? QueueNamesLoaded;

    public event EventHandler<string>? MessageSent;

    public event EventHandler<Exception>? MessageError;

    public async Task OnInitializedAsync()
    {
        Connections = await _connectionManager.GetValuesAsync();
    }

    public void SelectConnection(Connection? connection)
    {
        _queueNames.Clear();
        if (connection is not null)
        {
            SelectedConnection = connection;
            if (SelectedConnection is not null)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                LoadQueueNamesAsync(SelectedConnection);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }
        else
        {
            SelectedConnection = null;
        }
    }

    private async Task LoadQueueNamesAsync(Connection connection)
    {
        var queueNames = await _serviceBus
            .GetQueueNames(connection, CancellationToken.None);
        _queueNames.AddRange(queueNames);
        MessageDefinitions = await _messageDefinitionManager.GetValuesAsync();
        QueueNamesLoaded?.Invoke(this, EventArgs.Empty);
    }

    public void SelectQueueName(object? value)
    {
        if (value is string queueName)
        {
            SelectedQueueName = queueName;
        }
    }

    public void SelectMessageDefinition(MessageDefinition messageDefinition)
    {
        SelectedMessage = messageDefinition.Body;
        OnMessageChanged(SelectedMessage);
    }

    public void OnMessageChanged(object? args) {
        if (args is string body)
        {
            var message = new MessageDefinition
            {
                Body = body
            };
            var parameters = message.FindParameters();
            var parametersToRemove = MessageParameters
                .Where(k => !parameters.Contains(k.Name))
                .ToList();
            foreach (var parameter in parametersToRemove)
            {
                MessageParameters.Remove(parameter);
            }
            var parametersToAdd = parameters
                .Where(p => MessageParameters.All(mp => mp.Name != p))
                .Select(p => new Parameter
                {
                    Name = p,
                    Value = string.Empty
                })
                .ToList();
            MessageParameters.AddRange(parametersToAdd);
            _logger.LogDebug("Found parameters: {Parameters}", JsonSerializer.Serialize(MessageParameters));
        }
        else
        {
            MessageParameters.Clear();
        }
    }

    public async Task SendMessageAsync()
    {
        if (SelectedConnection is null)
        {
            _logger.LogError("No connection selected.");
            return;
        }
        if (SelectedQueueName is null)
        {
            _logger.LogError("No queue is selected.");
            return;
        }
        var message = SelectedMessage;
        foreach (var parameter in MessageParameters)
        {
            message = message.Replace($"%{parameter.Name}%", parameter.Value, StringComparison.InvariantCultureIgnoreCase);
        }
        try
        {
            await _serviceBus.SendMessageAsync(SelectedConnection, SelectedQueueName, SelectedMessage, MessageParameters, CancellationToken.None);
            _logger.LogInformation("Message {Message} sent", message);
            MessageSent?.Invoke(this, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message: {exception}", ex);
            MessageError?.Invoke(this, ex);
        }
    }
}