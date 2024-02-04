using System.Text.Json;

using Google.Api.Gax.ResourceNames;
using Google.Cloud.Translate.V3;
using Google.Protobuf.Collections;

using RVocabular.Models;


namespace RVocabular;

public class TranslateService
{
    public async Task<TranslationResponse> Translate(string word)
    {
        if (word is null) return null;

        using var httpClient = new HttpClient();

        var request = new HttpRequestMessage();

        request.RequestUri = new Uri($"http://api.datamuse.com/words?ml={word}&qe=ml&md=d&max=5");
        request.Method = HttpMethod.Get;

        var synonymsResponse = await httpClient.SendAsync(request);

        var content = await synonymsResponse.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var datamuseResponse = JsonSerializer
            .Deserialize<DemuseResponse[]>(content, options)
            .Select(x => new TranslationItem
            {
                Value = x.Word,
                Definitions = NormalizeDefinitions(x.Defs),
                PartsOfSpeech = x.Tags
                    .Where(t => _partOfSpeechTags.Contains(t))
                    .Select(t => _partsOfSpeeches[t])
                    .ToList()
            })
            .ToArray();

        var words = datamuseResponse.Select(x => x.Value).ToList();

        var translations = await GoogleTranslate(words);


        for (int i = 0; i < datamuseResponse.Length; i++)
        {
            datamuseResponse[i].TranslatedText = translations[i].TranslatedText;
        }

        var response = new TranslationResponse
        {
            MainResult = datamuseResponse[0],
            Synonyms = datamuseResponse[1..]
        };

        return response;
    }


    private async Task<RepeatedField<Translation>> GoogleTranslate(IReadOnlyList<string> contents)
    {
        var client = await TranslationServiceClient.CreateAsync();

        var request = new TranslateTextRequest
        {
            Contents = { contents },
            TargetLanguageCode = "uk-UK",
            SourceLanguageCode = "en-US",
            MimeType = "text/plain",
            Parent = new ProjectName("manifest-sum-402511").ToString()
        };

        var response = await client.TranslateTextAsync(request);

        return response.Translations;
    }


    private Definition[] NormalizeDefinitions(string[] value) =>
        value?
            .Select(x =>
            {
                var defItems = x.Split("\t");

                var tags = Array.Empty<string>();

                if (defItems[1].Contains('('))
                {
                    tags = defItems[1]
                        .Split('(',')')[1]
                        .Split(',');

                    defItems[1] = defItems[1].Split(')')[1];
                }

                return new Definition
                {
                    PartOfSpeech = _partsOfSpeeches[defItems[0].ToLower()],
                    Value = defItems[1],
                    Tags = tags
                };
            })
            .DistinctBy(x => x.PartOfSpeech)
            .ToArray();



    private readonly Dictionary<string, PartOfSpeech> _partsOfSpeeches = new()
    {
        { "n", PartOfSpeech.Noun },
        { "v", PartOfSpeech.Verb },
        { "adj", PartOfSpeech.Adjective },
        { "adv", PartOfSpeech.Adverb },
        { "u", PartOfSpeech.None },
    };

    private readonly string[] _partOfSpeechTags = { "n", "v", "adj", "adv", "u" };
}
