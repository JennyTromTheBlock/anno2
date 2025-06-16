using Application.Interfaces.Services;

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

builder.Services.AddSingleton<IElasticSearchService>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var elasticUrl = configuration["ELASTICSEARCH_URL"];
    return new ElasticSearchService(elasticUrl!);
});



var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.MapControllers();


app.Run();

