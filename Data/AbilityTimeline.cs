// Represents the status of a job availability on the timeline. 
public enum JobAbilityStatus
{
    Available,

    Active,

    OnCooldown,

    Unavailable
}

// Represents the timeline for the use and availability of a single ability for a single class. 
public class AbilityTimeline
{
    private readonly TimeSpan length;
    private readonly JobAbility ability;

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

    public List<JobTimelineEntry> AbilityUses = new List<JobTimelineEntry>();

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

        this.AbilityUses.Add(new JobTimelineEntry()
        {
            AbilityName = this.ability.Name,
            CastTime = useTime,
        });

        this.AbilityUses = this.AbilityUses.OrderBy(use => use.CastTime).ToList();

        this.UpdateAbilityTimeline();
    }

    public void RemoveAbiityUse(TimeSpan useTime)
    {
        JobTimelineEntry entry = this.AbilityUses.Find(use => use.CastTime == useTime);

        if (entry != null)
        {
            this.AbilityUses.Remove(entry);
            this.UpdateAbilityTimeline();
        }
    }

    private void UpdateAbilityTimeline()
    {
        foreach (TimeSpan second in this.AvailabilityTimeline.Keys)
        {
            JobAbilityStatus status = JobAbilityStatus.Available;

            TimeSpan? mostRecentUse = this.AbilityUses.LastOrDefault(use => use.CastTime <= second)?.CastTime;

            if (mostRecentUse != null)
            {
                if (mostRecentUse == second)
                {
                    status = JobAbilityStatus.Active;
                }
                else if (this.ability.Duration.HasValue && mostRecentUse + this.ability.Duration.Value > second)
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
                TimeSpan? nextUse = this.AbilityUses.FirstOrDefault(use => use.CastTime > second)?.CastTime;

                if (nextUse.HasValue && second + this.ability.Cooldown > nextUse.Value)
                {
                    status = JobAbilityStatus.Unavailable;
                }
            }

            this.AvailabilityTimeline[second] = status;
        }
    }
}