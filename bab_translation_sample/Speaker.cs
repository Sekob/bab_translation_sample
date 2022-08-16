using System.Threading.Tasks;
using System;

public class Speaker: IChatProcessing
{
    private readonly AzureSpeachProcessor _azureSpeechProcessor;
    private readonly IChat _chat;
    private readonly IChatParticipant _chatParticipant;
    public Speaker(AzureSpeachProcessor azureSpeechProcessor, IChat chat, IChatParticipant participant)
    {
        _azureSpeechProcessor = azureSpeechProcessor;
        _chat = chat;
        _chatParticipant = participant;
    }

    public async Task Process()
    {
        System.Console.WriteLine("Listening...");
        var text = await _azureSpeechProcessor.CaptureSpeechAsTextAsync();
        Console.WriteLine(text);
        await _chat.SendMessageAsync(new AzureChatMessage{Text = text, SenderName = _chatParticipant.Name});
    }
}