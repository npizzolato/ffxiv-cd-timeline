public class JobAbility
{
    public string Name { get; set; }

    public TimeSpan? Duration { get; set; }

    public TimeSpan Cooldown { get; set; }

    public double Mitigation { get; set; }

    public JobAbilityType Type { get; set; }
}
