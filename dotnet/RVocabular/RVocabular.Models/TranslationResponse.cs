namespace RVocabular.Models;

public class TranslationResponse
{
    public TranslationItem MainResult { get; set; }

    public IReadOnlyList<TranslationItem> Synonyms { get; set; }
}

public class TranslationItem
{
    public string Value { get; set; }

    public string TranslatedText { get; set; }

    public IReadOnlyList<PartOfSpeech> PartsOfSpeech { get; set; }

    public IReadOnlyList<Definition> Definitions { get; set; }
}

public class Definition
{
    public PartOfSpeech PartOfSpeech { get; set; }

    public string Value { get; set; }

    public string[] Tags { get; set; }
}
