using System.Threading.Tasks;
using System.Collections.Generic;

public interface IChatService
{
    Task<IChat> ConnectToChat(string title, IList<IChatParticipant> participants);
    Task<IChat> ConnectToChat(string title, IChatParticipant participant);
}
