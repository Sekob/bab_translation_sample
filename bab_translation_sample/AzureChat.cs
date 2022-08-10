using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Communication.Chat;

public class AzureChat : IChat
{
    private readonly ChatThreadClient _threadClient;
    private DateTimeOffset? _lastMessageTime = null;
    public AzureChat(ChatThreadClient threadClient)
    {
        _threadClient = threadClient;
    }
    public async Task<IList<IChatMessage>> GetMessagesAsync()
    {
        var resultMessages = new List<AzureChatMessage>();
        AsyncPageable<ChatMessage> messages = _threadClient.GetMessagesAsync(_lastMessageTime);
        await foreach(var message in messages)
        {
            resultMessages.Add(new AzureChatMessage {
                SenderName = message.SenderDisplayName,
                Text = message.Content.Message,
                Metadata = message.Metadata
            });
            _lastMessageTime = message.CreatedOn;
        }
       return (IList<IChatMessage>)resultMessages; 
    }

    public async Task<IReadOnlyCollection<IChatParticipant>> GetParticipants()
    {
        var chatParticipant = new List<ChatParticipant>();
        var participants = _threadClient.GetParticipantsAsync();
        await foreach(var participant in participants)
        {
            chatParticipant.Add(participant);
        }
        return chatParticipant.Select(p=> new AzureChatParticipant{Name = p.DisplayName}).ToList();
    }

    // TODO: need support different chat message types
    public async Task SendMessageAsync(IChatMessage message)
    {
        await _threadClient.SendMessageAsync(message.Text, ChatMessageType.Text, message.SenderName);
    }
}