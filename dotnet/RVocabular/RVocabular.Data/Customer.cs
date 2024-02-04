using LinqToDB.Mapping;


namespace RVocabular.Data;

[Table]
public class Customer
{
    [Column, PrimaryKey, Identity]
    public long Id { get; set; }

    [Column]
    public string TelegramChatId { get; set; }

    [Column]
    public DateTime? LastLessonAt { get; set; }
}
