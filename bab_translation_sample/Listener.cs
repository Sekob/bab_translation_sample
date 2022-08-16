using System.Threading.Tasks;
using System;

public class Listener : IChatProcessing
{
    private readonly AzureSpeachProcessor _speachProcessor;
    private readonly AzureTextTranslater _translater;
    private readonly IChat _chat;
    public Listener(AzureSpeachProcessor azureSpeachProcessor, AzureTextTranslater translater, IChat chat)
    {
        _speachProcessor = azureSpeachProcessor;
        _translater = translater;
        _chat = chat;
    }
    public async Task Process()
    {
        var messages = await _chat.GetMessagesAsync();
        foreach(var message in messages)
        {
            var translatedText = await _translater.TranslateAsync("ru", "en", message.Text);
            if (translatedText != null)
            {
                Console.WriteLine(translatedText);
                await _speachProcessor.SpeekTextAsync(translatedText.Text);
            }
        }
    }
}