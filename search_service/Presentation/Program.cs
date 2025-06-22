using Application.Interfaces.Services;
using DefaultNamespace;
using Nest;

var builder = WebApplication.CreateBuilder(args);



// ✅ Load environment variables from .env
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
    var configuration = sp.GetRequiredService<IConfiguration>();
    var elasticUrl = configuration["ELASTICSEARCH_URL"]!;
    
    var settings = new ConnectionSettings(new Uri(elasticUrl))
        .DefaultIndex("default-index"); // evt. standardindex

    return new ElasticClient(settings);
});

builder.Services.AddSingleton<ElasticIndexManager>();

// ElasticIndexManager og service
builder.Services.AddSingleton<IElasticSearchService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var elasticUrl = config["ELASTICSEARCH_URL"] ?? throw new InvalidOperationException("Missing ELASTICSEARCH_URL");
    
    var client = sp.GetRequiredService<IElasticClient>();
    var indexManager = sp.GetRequiredService<ElasticIndexManager>();

    return new ElasticSearchService(elasticUrl, indexManager);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.MapControllers();


app.Run();

