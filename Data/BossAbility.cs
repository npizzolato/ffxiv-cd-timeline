using System.Text.Json.Serialization;

// Represents an individual ability used by a boss/enemy. 
public class BossAbility
{
    public string Name { get; set; }

    public string Description { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AbilityCategory Category { get; set; }
}