using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

public class AzureSpeachProcessor : ISpeechProcessor, IDisposable
{
    public class AuzreSpeechProcessorConfiguration
    {
      public string SpeechRecognitionLanguage { get; init; }
      public string SubscriptionKey { get; init; }
      public string Region { get; init; }
    }

    //TODO: AudioConfigs should be injectable
    private readonly AudioConfig _audioIputConfig;
    private readonly AudioConfig _audioOutputConfig;
    private readonly SpeechConfig _speechConfig;
    private readonly SpeechRecognizer _speechRecognizer;
    private readonly SpeechSynthesizer _speechSynthesizer;
    private bool disposedValue;

    public AzureSpeachProcessor(AzureSpeachProcessor.AuzreSpeechProcessorConfiguration configuration)
    {
        _speechConfig = SpeechConfig.FromSubscription(configuration.SubscriptionKey, configuration.Region);
        _speechConfig.SpeechRecognitionLanguage = configuration.SpeechRecognitionLanguage;
        _audioIputConfig = AudioConfig.FromDefaultMicrophoneInput(); //TODO: make it possible to choose microphone
        _audioOutputConfig = AudioConfig.FromDefaultSpeakerOutput();
        _speechRecognizer = new SpeechRecognizer(_speechConfig, _audioIputConfig);
        _speechSynthesizer = new SpeechSynthesizer(_speechConfig, _audioOutputConfig);
    }

    public async Task<string> CaptureSpeechAsTextAsync()
    {
        var speechRecognitionResult = await _speechRecognizer.RecognizeOnceAsync();
        return speechRecognitionResult.Reason switch  {
            ResultReason.RecognizedSpeech => speechRecognitionResult.Text,
            _ => String.Empty
        };
    }

    public async Task SpeekTextAsync(string text)
    {
        //TODO: make it configurable and generated randomly
        var ssml =
            $"<speak version=\"1.0\" xmlns=\"http://www.w3.org/2001/10/synthesis\" xmlns:mstts=\"https://www.w3.org/2001/mstts\" xml:lang=\"en-US\"> <voice name=\"en-US-SaraNeural\"> <mstts:express-as style=\"cheerful\">{text}</mstts:express-as></voice></speak>";
        await _speechSynthesizer.SpeakSsmlAsync(ssml);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            _speechRecognizer.Dispose();
            _speechSynthesizer.Dispose();
            _audioIputConfig.Dispose();
            _audioOutputConfig.Dispose();
            disposedValue = true;
        }
    }

    ~AzureSpeachProcessor()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
