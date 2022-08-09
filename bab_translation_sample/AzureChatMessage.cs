using System.Collections.Generic;

public class AzureChatMessage : IChatMessage
{
    public string Text { get; init; }

    public string SenderName { get; init; }

    public IReadOnlyDictionary<string, string> Metadata { get; init; }
}