using System.Text.Json.Serialization;

public class BossAbility
{
    public string Name { get; set; }

    public string Description { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AbilityCategory Category { get; set; }
}