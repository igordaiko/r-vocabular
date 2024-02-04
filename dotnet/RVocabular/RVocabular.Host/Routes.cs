using Microsoft.AspNetCore.Mvc;

using RVocabular.Models;


namespace RVocabular.Host;

public static class Routes
{
    public static void RegisterRoutes(this WebApplication app)
    {
        app.MapGet("/", ([FromServices] Settings settings) =>
        {
            return $"From Instance {settings.MachineId}";
        });

        app.MapGet("/translate", ([FromQuery] string word, [FromServices] TranslateService service) =>
        {
            return service.Translate(word);
        });


        app.MapPut("/add", ([FromServices] VocabularyService service, [FromBody] AddWordRequest request) =>
        {
            return service.AddNewWord(request);
        });
    }
}
