namespace RVocabular.Host;

public class Settings
{
    public HttpSettings Http { get; set; }

    public ServiceSettings Service { get; set; }

    public string MachineId { get; set; }
}

public class HttpSettings
{
    public string BasePath { get; set; } = "/";
}
