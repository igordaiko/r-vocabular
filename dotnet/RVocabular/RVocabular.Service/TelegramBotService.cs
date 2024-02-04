using LinqToDB;
using LinqToDB.DataProvider.PostgreSQL;

using RVocabular.Data;

using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;


namespace RVocabular;

public class TelegramBotService
{
    public TelegramBotService(Database database, ServiceSettings settings)
    {
        _database = database;
        _settings = settings;
    }


    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not {} message)
            return;

        if (message.Text is not {} messageText)
            return;

        var chatId = message.Chat.Id;

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "You said:\n" + messageText,
            cancellationToken: cancellationToken);
    }


    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);

        return Task.CompletedTask;
    }


    public async Task Notify(ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var t1 = await (
                from c in _database.Customers()
                // join w in _database.Vocabulary() on c.Id equals w.CustomerId
                from w in _database.Vocabulary().Where(x => x.CustomerId == c.Id).DefaultIfEmpty()
                where
                    (c.LastLessonAt == null || (now - c.LastLessonAt).Value >= _settings.Education.GapBetweenLessons) &&
                    (w.LastReviewedAt == null || (now - (w.LastReviewedAt ?? w.AddedAt)) >= _settings.Education.GapBetweenWordRepeating)
                select new
                {
                    c.Id,
                    c.TelegramChatId,
                    w.Word,
                    w.Translation
                })
            .ToListAsync();

        var customers = await (
                from c in _database.Customers()
                from w in _database.Vocabulary().Where(x => x.CustomerId == c.Id).DefaultIfEmpty()
                where
                    (c.LastLessonAt == null || (now - c.LastLessonAt).Value >= _settings.Education.GapBetweenLessons) &&
                    (w.LastReviewedAt == null || (now - (w.LastReviewedAt ?? w.AddedAt)) >= _settings.Education.GapBetweenWordRepeating)
                select new
                {
                    c.Id,
                    c.TelegramChatId,
                    w.Word,
                    w.Translation
                }
                into cw
                group new { cw.Word, cw.Translation } by new { cw.Id, cw.TelegramChatId }
                into g
                select new
                {
                    g.Key.Id,
                    g.Key.TelegramChatId,
                    Words = g.ArrayAggregate(x => new { x.Word, x.Translation }, Sql.AggregateModifier.None).ToValue()
                }
            )
            .ToListAsync(token: cancellationToken);

        var t = 1;

        // if (customers.Count == 0)
        //     return;
        //
        // foreach (var customer in customers)
        // {
        //     await botClient.SendTextMessageAsync(
        //         chatId: customer.TelegramChatId,
        //         text: string.Join(",", customer.Words.Select(x => $"{x.Word} - {x.Translation}")),
        //         cancellationToken: cancellationToken
        //     );
        //
        //     await _database.Vocabulary()
        //         .Where(x => x.CustomerId == customer.Id && customer.Words.Select(x => x.Word).Contains(x.Word))
        //         .Set(x => x.LastReviewedAt, now)
        //         .UpdateAsync(token: cancellationToken);
        // }
        //
        // await _database.Customers()
        //     .Where(x => customers.Select(x => x.Id).Contains(x.Id))
        //     .Set(x => x.LastLessonAt, now)
        //     .UpdateAsync(token: cancellationToken);
    }


    public async Task<DateTime?> NextMessageAt()
    {
        var lastLessonsAt = await _database.Customers()
            .Select(x => x.LastLessonAt)
            .ToListAsync();

        return lastLessonsAt.Min(x => x);
    }


    private readonly Database _database;

    private readonly ServiceSettings _settings;
}
