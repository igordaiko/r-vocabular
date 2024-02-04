using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;


namespace RVocabular.Bot;

public class Settings
{
    public TelegramApiSettings TelegramApi { get; set; } = new();

    public ServiceSettings Service { get; set; } = new();
}

public class TelegramApiSettings
{
    public string AccessToken { get; set; }

    public ReceiverOptions ReceiverOptions = new ()
    {
        AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
    };
}
