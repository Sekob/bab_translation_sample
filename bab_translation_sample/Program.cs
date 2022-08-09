using System;

const string subscriptionKey = "b43ff619a87649089b35854e385bf4a3";
const string location = "global";
const string translationError = "Something wrong have happened during translation";

var configuration = new AzureSpeachProcessor.AuzreSpeechProcessorConfiguration
{
    Region = "northeurope",
    SpeechRecognitionLanguage = "ru-RU",
    SubscriptionKey = "9754f231122844279963d0dff225648f"
};

using var azureSpeechProcessor = new AzureSpeachProcessor(configuration);

Console.WriteLine("Listening...");

var text = await azureSpeechProcessor.CaptureSpeechAsTextAsync();

Console.WriteLine(text); 

using var translator = new AzureTextTranslater(subscriptionKey, location);
var translatedText = (await translator.TranslateAsync("ru", "en", text))?.Text;
// Send text to another participant
Console.WriteLine(translatedText switch { null => translationError, not null => translatedText });
// Get text from speaker (another participant) and speek it with voice morphing
await azureSpeechProcessor.SpeekTextAsync(translatedText);
