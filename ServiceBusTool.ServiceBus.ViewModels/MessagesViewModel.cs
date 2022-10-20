using Microsoft.Extensions.Logging;
using ServiceBusTool.ServiceBus.Contract.Interfaces;
using ServiceBusTool.ServiceBus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusTool.ServiceBus.ViewModels
{
    public class MessagesViewModel
    {
        private readonly IKeyValueListManager<MessageDefinition> _messageDefinitionManager;
        private readonly ILogger<MessagesViewModel> _logger;

        private readonly List<MessageDefinition> _messageDefinitions = new();
        private readonly List<string> _messageParameters = new();

        public MessagesViewModel(
            IKeyValueListManager<MessageDefinition> messageDefinitionManager,
            ILogger<MessagesViewModel> logger)
        {
            _messageDefinitionManager = messageDefinitionManager;
            _logger = logger;
        }

        public IReadOnlyList<MessageDefinition> MessageDefinitions => _messageDefinitions;

        public IReadOnlyList<string> MessageParameters => _messageParameters;

        public MessageDefinition? SelectedMessageDefinition { get; private set; }

        public async Task OnInitializedAsync()
        {
            await LoadConnections();
        }

        private async Task LoadConnections()
        {
            SelectedMessageDefinition = null;
            _messageDefinitions.Clear();
            var connections = (await _messageDefinitionManager
                .GetValuesAsync())
                .OrderBy(c => c.Name)
                .ToList();
            _messageDefinitions.AddRange(connections);
        }

        public void OnMessageDefinitionSelected(MessageDefinition messageDefinition)
        {
            SelectedMessageDefinition = messageDefinition;
        }

        public void AddMessageDefinition()
        {
            SelectedMessageDefinition = new MessageDefinition();
        }

        public async Task OnSubmit()
        {
            if (SelectedMessageDefinition is null)
            {
                _logger.LogError("Message definition saved, but selectedMessageDefinition is null.");
                return;
            }
            await _messageDefinitionManager.SaveValueAsync(SelectedMessageDefinition);
            await LoadConnections();
        }

        public void OnBodyChanged(object? body)
        {
            _messageParameters.Clear();
            if (SelectedMessageDefinition is null)
            {
                return;
            }
            SelectedMessageDefinition.Body = body?.ToString() ?? string.Empty;
            var parameters = SelectedMessageDefinition.FindParameters();
            _messageParameters.AddRange(parameters);
        }
    }
}
