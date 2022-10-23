﻿using ServiceBusTool.ServiceBus.Contract.Interfaces;
using ServiceBusTool.ServiceBus.Models;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ServiceBusTool.ServiceBus.ViewModels
{
    public class IndexViewModel
    {
        private readonly IKeyValueListManager<Connection> _connectionManager;
        private readonly IServiceBus _serviceBus;
        private readonly ILogger<IndexViewModel> _logger;

        private readonly List<string> _queueNames = new();

        public IndexViewModel(
            IKeyValueListManager<Connection> connectionManager,
            IServiceBus serviceBus,
            ILogger<IndexViewModel> logger)
        {
            _connectionManager = connectionManager;
            _serviceBus = serviceBus;
            _logger = logger;
        }

        public IEnumerable<Connection>? Connections { get; private set; }
        public Connection? SelectedConnection { get; private set; }
        public IEnumerable<string> QueueNames => _queueNames;
        public string? SelectedQueueName { get; private set; }



        public MessageDefinition SelectedMessageDefinition { get; } = new();
        public List<Parameter> MessageParameters { get; } = new();

        public async Task OnInitializedAsync()
        {
            Connections = await _connectionManager.GetValuesAsync();
        }

        public async Task SelectConnectionAsync(object? arg)
        {
            _queueNames.Clear();
            if (arg is string value && Guid.TryParse(value, out Guid connectionId))
            {
                SelectedConnection = Connections?.FirstOrDefault(c => c.Id == connectionId);
                if (SelectedConnection is not null)
                {
                    var queueNames = await _serviceBus
                        .GetQueueNames(SelectedConnection, CancellationToken.None);
                    _queueNames.AddRange(queueNames);
                }
            }
            else
            {
                SelectedConnection = null;
            }
        }

        public void SelectQueueName(object? value)
        {
            if (value is string queueName)
            {
                SelectedQueueName = queueName;
            }
        }

        private void SelectMessageDefinition(MessageDefinition messageDefinition)
        {
            SelectedMessageDefinition.Body = messageDefinition.Body;
        }

        public void OnMessageChanged(object? args)
        {
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

        public void SendMessage()
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
            var message = SelectedMessageDefinition.Body;
            foreach (var parameter in MessageParameters)
            {
                message = message.Replace($"%{parameter.Name}%", parameter.Value, StringComparison.InvariantCultureIgnoreCase);
            }
            _serviceBus.SendMessage(SelectedConnection, SelectedQueueName, SelectedMessageDefinition.Body, MessageParameters, CancellationToken.None);
            _logger.LogInformation("Message {Message} sent", message);
        }
    }
}