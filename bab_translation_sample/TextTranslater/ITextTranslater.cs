using System.Threading.Tasks;

public interface ITextTranslater
{
    Task<ITextTranslatedData> TranslateAsync(string from, string to, string text);
}
