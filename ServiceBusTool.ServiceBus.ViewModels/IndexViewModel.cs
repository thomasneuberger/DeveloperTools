using ServiceBusTool.ServiceBus.Contract.Interfaces;
using ServiceBusTool.ServiceBus.Models;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ServiceBusTool.ServiceBus.ViewModels
{
    public class IndexViewModel
    {
        private readonly IKeyValueListManager<Connection> _connectionManager;
        private readonly ILogger<IndexViewModel> _logger;

        public IndexViewModel(IKeyValueListManager<Connection> connectionManager, ILogger<IndexViewModel> logger)
        {
            _connectionManager = connectionManager;
            _logger = logger;
        }

        public IEnumerable<Connection>? Connections { get; private set; }
        public Connection? SelectedConnection { get; private set; }

        public MessageDefinition SelectedMessageDefinition { get; } = new();
        public List<Parameter> MessageParameters { get; } = new();

        public async Task OnInitializedAsync()
        {
            Connections = await _connectionManager.GetValuesAsync();
        }

        public void SelectConnection(object? arg)
        {
            if (arg is string value && Guid.TryParse(value, out Guid connectionId))
            {
                SelectedConnection = Connections?.FirstOrDefault(c => c.Id == connectionId);
            }
            else
            {
                SelectedConnection = null;
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
            var message = SelectedMessageDefinition.Body;
            foreach (var parameter in MessageParameters)
            {
                message = message.Replace($"%{parameter.Name}%", parameter.Value, StringComparison.InvariantCultureIgnoreCase);
            }
            _logger.LogInformation("Send message {Message}", message);
        }
    }
}