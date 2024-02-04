using LinqToDB;

using RVocabular.Data;
using RVocabular.Models;


namespace RVocabular;

public class VocabularyService
{
    public VocabularyService(Database database)
    {
        _database = database;
    }

    public async Task<bool> AddNewWord(AddWordRequest request)
    {
        if (request.Word is null || (request.Translation is null && request.Definition is null))
            return false;

        await _database
            .InsertAsync(new Vocabulary
            {
                CustomerId = CustomerId,
                Word = request.Word,
                Translation = request.Translation,
                Definition = request.Definition,
                AddedAt = DateTime.UtcNow
            });

        await _database.CommitTransactionAsync();

        return true;
    }


    // TODO: Add customer session
    private const int CustomerId = 1;

    private readonly Database _database;
}
