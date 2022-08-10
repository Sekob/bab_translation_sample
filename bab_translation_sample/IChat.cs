using System.Threading.Tasks;
using System.Collections.Generic;

public interface IChat
{
    Task SendMessageAsync(IChatMessage message);
    Task<IList<IChatMessage>> GetMessagesAsync();
    Task<IReadOnlyCollection<IChatParticipant>> GetParticipants();
}
