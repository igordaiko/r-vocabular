using System.Text.Json;
using System.Text.Json.Serialization;

using LinqToDB;
using LinqToDB.Data;

using HttpJsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

using RVocabular;
using RVocabular.Data;
using RVocabular.Host;


var builder = WebApplication.CreateBuilder(args);

var settings = builder
    .Configuration
    .Get<Settings>();


ConfigureHost();
ConfigureServices();

void ConfigureHost()
{
    builder.Host.ConfigureContainer<IServiceCollection>(static (_, services) =>
    {
        services.Configure<HttpJsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));;
        });
    });
}


void ConfigureServices()
{
    var services = builder.Services;

    services.AddSingleton(settings ?? new());

    services.AddScoped<TranslateService>();
    services.AddScoped<VocabularyService>();

    services.AddScoped(_ =>
    {
        var dbSettings = settings!.Service.Database;

        var options = new DataOptions()
            .UseConnectionString(dbSettings.DataProvider, dbSettings.ConnectionString)
            .WithOptions<ConnectionOptions>(o => o.WithOnEntityDescriptorCreated((schema, entity) =>
            {
                o.OnEntityDescriptorCreated?.Invoke(schema, entity);

                entity.TableName = entity.TableName.ToUnderscore();

                foreach (var column in entity.Columns)
                    column.ColumnName = column.ColumnName.ToUnderscore();
            }));

        return new Database(options);
    });
}

var app = builder.Build();


// AddGoogleEnvironmentVariable();

app.RegisterRoutes();

app.Run();

void AddGoogleEnvironmentVariable()
{
    var GAC = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");

    if (GAC == null)
    {
        var path = @"./manifest-sum-402511-b9eec11b65fe.json";

        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
    }
}
