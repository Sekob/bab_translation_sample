using System.Threading.Tasks;
using System.Collections.Generic;
using Azure;
using Azure.Communication;
using Azure.Communication.Chat;
using System;
public class AzureChatService : IChatService
{

    private readonly ChatClient _chatClient;
    public AzureChatService(Uri endpoint, string accessToken)
    {
        var communicationTokenCredential = new CommunicationTokenCredential(accessToken);
        _chatClient = new ChatClient(endpoint, communicationTokenCredential);
    }

    public async Task<IChat> ConnectToChat(string title, IList<IChatParticipant> participants)
    {
        var chatParticipants = new List<ChatParticipant>(participants.Count);
        foreach (var participant in participants)
        {
            chatParticipants.Add(
                new ChatParticipant(identifier: new CommunicationUserIdentifier(id: participant.Token))
                {
                    DisplayName = participant.Name
                });
        }
        CreateChatThreadResult createChatThreadResult = await _chatClient.CreateChatThreadAsync(topic: title, participants: chatParticipants);
        ChatThreadClient chatThreadClient = _chatClient.GetChatThreadClient(threadId: createChatThreadResult.ChatThread.Id);
        return new AzureChat(chatThreadClient);
    }

    public async Task<IChat> ConnectToChat(string title, IChatParticipant participant)
    {
        return await ConnectToChat(title, new[] { participant });
    }
}
