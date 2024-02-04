using Telegram.Bot;


namespace RVocabular.Bot.HostedServices;

public class NotificationService : BackgroundService
{
    public NotificationService(TelegramApiSettings settings, TelegramBotService service)
    {
        _settings = settings;
        _service = service;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var botClient = new TelegramBotClient(_settings.AccessToken);

        botClient.StartReceiving(
            updateHandler: _service.HandleUpdateAsync,
            pollingErrorHandler: _service.HandlePollingErrorAsync,
            receiverOptions: _settings.ReceiverOptions, cancellationToken: stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var nextMessageAt = await _service.NextMessageAt() ?? now;

            if (nextMessageAt <= now)
                await _service.Notify(botClient, stoppingToken);
            else
                await Task.Delay((nextMessageAt - now).Milliseconds, stoppingToken);
        }
    }


    private readonly TelegramApiSettings _settings;

    private readonly TelegramBotService _service;
}
