using LinqToDB.Mapping;


namespace RVocabular.Data;

[Table]
public class Vocabulary
{
    [Column]
    public long CustomerId { get; set; }

    [Column]
    public string Word { get; set; }

    [Column]
    public DateTime AddedAt { get; set; }

    [Column]
    public string Translation { get; set; }

    [Column]
    public string Definition { get; set; }

    [Column]
    public DateTime? LastReviewedAt { get; set; }
}
