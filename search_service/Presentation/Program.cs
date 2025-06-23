using Application.Domains.Enums;
using Application.Indexers;
using Application.Interfaces.Services;
using Application.Services;
using Nest;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env
if (File.Exists("../../.env"))
{
    foreach (var line in File.ReadAllLines("../.env"))
    {
        if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
            continue;

        var parts = line.Split('=', 2);
        if (parts.Length == 2)
        {
            Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
        }
    }
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IElasticClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var elasticUrl = config["ELASTICSEARCH_URL"] 
        ?? throw new InvalidOperationException("Missing ELASTICSEARCH_URL");

    var settings = new ConnectionSettings(new Uri(elasticUrl))
        .DefaultIndex(ElasticIndex.PdfWords.ToString().ToLower());

    return new ElasticClient(settings);
});


builder.Services.AddSingleton<ElasticIndexManager>();
builder.Services.AddSingleton<IElasticSearchService, ElasticSearchService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();

