// Represents the metadata for a single ability used by a job.
public class JobAbility
{
    public string Name { get; set; }

    public TimeSpan? Duration { get; set; }

    public TimeSpan Cooldown { get; set; }

    public double Mitigation { get; set; }

    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public JobAbilityType Type { get; set; }

    public int Charge { get; set; } = 1;
}
