using System;
using System.Collections.Generic;
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

    public IReadOnlyCollection<IChatParticipant> GetParticipants()
    {
        throw new System.NotImplementedException();
    }

    public Task SendMessageAsync(IChatMessage message)
    {
        throw new System.NotImplementedException();
    }
}