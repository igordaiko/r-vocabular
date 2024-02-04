namespace RVocabular;

public class ServiceSettings
{
    public DatabaseSettings Database { get; set; } = new();

    public EducationSettings Education { get; set; } = new();
}

public class DatabaseSettings
{
    public string DataProvider { get; set; } = LinqToDB.ProviderName.PostgreSQL;

    public string ConnectionString { get; set; }
}

public class EducationSettings
{
    public TimeSpan GapBetweenLessons { get; set; } = TimeSpan.FromMinutes(1);

    public TimeSpan GapBetweenWordRepeating { get; set; } = TimeSpan.FromHours(1);
}
