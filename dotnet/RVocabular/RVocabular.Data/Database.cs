using LinqToDB;
using LinqToDB.Data;


namespace RVocabular.Data;

public class Database : DataConnection
{
    public Database(DataOptions options) : base(options)
    {
    }


    public ITable<Customer> Customers() => this.GetTable<Customer>();

    public ITable<Vocabulary> Vocabulary() => this.GetTable<Vocabulary>();
}
