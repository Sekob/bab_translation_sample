using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


public class AzureTextTranslater : ITextTranslater, IDisposable
{
    [Serializable]
    private class TranslationData
    {
        [Serializable]
        public class Translation
        {
            public string text { get; set; }
            public string to { get; set; }
        }

        public Translation[] translations { get; set; }
    }

    private readonly HttpClient _httpClient;
    private readonly string _subscriptionKey;
    private readonly string _location;
    private readonly string _endpoint = "https://api.cognitive.microsofttranslator.com/";
    private readonly Func<string, string, string> _createRoute = (from, to) => $"/translate?api-version=3.0&from={from}&to={to}";
    
    public AzureTextTranslater(string subscriptionKey, string location)
    {
        _subscriptionKey = subscriptionKey;
        _location = location;
        _httpClient = new HttpClient();
    }

    //TODO: need to remove strings and add enums for from and to languages
    public async Task<ITextTranslatedData> TranslateAsync(string from, string to, string text)
    {
        using var request = CreateRequest(from, to, text);
        HttpResponseMessage response = await _httpClient.SendAsync(request).ConfigureAwait(false);

        if(response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            return null;
        }

        string result = await response.Content.ReadAsStringAsync();
        var translatedData = JsonSerializer.Deserialize<TranslationData[]>(result);
        return new AzureTextTranslatedData(@from, to, translatedData![0]!.translations[0].text);
    }

    private HttpRequestMessage CreateRequest(string from, string to, string text)
    {
        string route = _createRoute(from, to);
        object[] body = { new { Text = text } };
        var requestBody = JsonSerializer.Serialize(body);

        var request = new HttpRequestMessage();

        request.Method = HttpMethod.Post;
        request.RequestUri = new Uri(_endpoint + route);
        request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        request.Headers.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
        request.Headers.Add("Ocp-Apim-Subscription-Region", _location);

        return request;
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool suppress)
    {
        if (suppress)
        {
            GC.SuppressFinalize(this);
        }

        _httpClient.Dispose();
    }

    ~AzureTextTranslater()
    {
        Dispose(false);
    }
}
