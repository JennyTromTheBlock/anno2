namespace Presentation.Messages.options;

public class RabbitMqOptions
{
    public string Host { get; set; } = "localhost";
    public string User { get; set; } = "guest";
    public string Pass { get; set; } = "guest";

    public string ConnectionString =>
        $"host={Host};username={User};password={Pass}";
}
