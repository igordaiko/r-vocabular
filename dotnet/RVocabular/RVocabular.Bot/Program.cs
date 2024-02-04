using LinqToDB;
using LinqToDB.Data;

using RVocabular;
using RVocabular.Bot;
using RVocabular.Bot.HostedServices;
using RVocabular.Data;

var builder = Host
    .CreateDefaultBuilder()
    .ConfigureServices(ConfigureServices)
    .Build();

builder.Run();

void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    var settings = context.Configuration.Get<Settings>();

    services.AddSingleton(settings.TelegramApi);
    services.AddSingleton(settings.Service);

    services.AddScoped(_ =>
    {
        var dbSettings = settings.Service.Database;

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

    services.AddScoped<TelegramBotService>();


    services.AddHostedService<NotificationService>();
}
