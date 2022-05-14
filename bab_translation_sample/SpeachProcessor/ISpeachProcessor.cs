using System.Threading.Tasks;
public interface ISpeechProcessor
{
    Task<string> CaptureSpeechAsTextAsync();
    Task SpeekTextAsync(string text);
}