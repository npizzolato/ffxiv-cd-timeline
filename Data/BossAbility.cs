using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

// Represents an individual ability used by a boss/enemy. 
public class BossAbility
{
    public string Name { get; set; }

    public string Description { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public AbilityCategory Category { get; set; }
}