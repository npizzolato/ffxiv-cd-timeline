public enum JobAbilityStatus
{
    Available,

    Active,

    OnCooldown,

    Unavailable
}

public class AbilityTimeline
{
    private readonly TimeSpan length;
    private readonly JobAbility ability;
    private List<JobTimelineEntry> abilityUses = new List<JobTimelineEntry>();

    public AbilityTimeline(TimeSpan length, JobAbility ability)
    {
        this.length = length;
        this.ability = ability;
        this.AvailabilityTimeline = new Dictionary<TimeSpan, JobAbilityStatus>();

        foreach (TimeSpan second in Enumerable.Range(0, Convert.ToInt32(length.TotalSeconds))
                .Select(multiplier => TimeSpan.Zero.Add(TimeSpan.FromSeconds(1 * multiplier))))
                {
                    this.AvailabilityTimeline[second] = JobAbilityStatus.Available;
                }
    }

    public IDictionary<TimeSpan, JobAbilityStatus> AvailabilityTimeline { get; }

    public void AddAbilityUse(TimeSpan useTime)
    {
        if (!this.AvailabilityTimeline.ContainsKey(useTime))
        {
            throw new ArgumentException($"The timeline does not support abilities used at time {useTime}.");
        }

        if (this.AvailabilityTimeline[useTime] != JobAbilityStatus.Available)
        {
            throw new ArgumentException($"Job ability {this.ability.Name} is not available to use at time {useTime}. It's current status is {this.AvailabilityTimeline[useTime]}");
        }

        this.abilityUses.Add(new JobTimelineEntry()
        {
            AbilityName = this.ability.Name,
            CastTime = useTime,
        });

        this.abilityUses = this.abilityUses.OrderBy(use => use.CastTime).ToList();

        this.UpdateAbilityTimeline();
    }

    public void RemoveAbiityUse(TimeSpan useTime)
    {
        JobTimelineEntry entry = this.abilityUses.Find(use => use.CastTime == useTime);

        if (entry != null)
        {
            this.abilityUses.Remove(entry);
            this.UpdateAbilityTimeline();
        }
    }

    private void UpdateAbilityTimeline()
    {
        foreach (TimeSpan second in this.AvailabilityTimeline.Keys)
        {
            JobAbilityStatus status = JobAbilityStatus.Available;

            TimeSpan? mostRecentUse = this.abilityUses.LastOrDefault(use => use.CastTime < second)?.CastTime;

            if (mostRecentUse != null)
            {
                if (this.ability.Duration.HasValue && mostRecentUse + this.ability.Duration.Value >= second)
                {
                    status = JobAbilityStatus.Active;
                }
                else if (mostRecentUse + this.ability.Cooldown > second)
                {
                    status = JobAbilityStatus.OnCooldown;
                }
            }

            // If it's still technically available, check if a later use makes the ability effectively unavailable for use right now.
            if (status == JobAbilityStatus.Available)
            {
                TimeSpan? nextUse = this.abilityUses.FirstOrDefault(use => use.CastTime > second)?.CastTime;

                if (nextUse.HasValue && second + this.ability.Cooldown > nextUse.Value)
                {
                    status = JobAbilityStatus.Unavailable;
                }
            }

            this.AvailabilityTimeline[second] = status;
        }
    }
}