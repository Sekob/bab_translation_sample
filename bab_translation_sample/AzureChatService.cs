using System.Threading.Tasks;
using System.Collections.Generic;

public class AzureChatService : IChatService
{
    public AzureChatService()
    {
    }

    public async Task<IChat> ConnectToChat(string title, IList<IChatParticipant> participants)
    {
        return null;
    }

    public async Task<IChat> ConnectToChat(string title, IChatParticipant participant)
    {
        return null;
    }
}
