using System.Collections.Generic;

public interface IChatMessage
{
    string Text { get; }
    string SenderName { get; }
    IReadOnlyDictionary<string, string> Metadata { get; }
}
