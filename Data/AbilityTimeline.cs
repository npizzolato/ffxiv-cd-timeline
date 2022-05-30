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
    private IDictionary<TimeSpan, JobAbilityStatus> timeline { get; }
    private List<JobTimelineEntry> abilityUses = new List<JobTimelineEntry>();

    public AbilityTimeline(TimeSpan length, JobAbility ability)
    {
        this.length = length;
        this.ability = ability;
        this.timeline = new Dictionary<TimeSpan, JobAbilityStatus>();

        foreach (TimeSpan second in Enumerable.Range(0, Convert.ToInt32(length.TotalSeconds))
                .Select(multiplier => TimeSpan.Zero.Add(TimeSpan.FromSeconds(1 * multiplier))))
                {
                    this.timeline[second] = JobAbilityStatus.Available;
                }
    }

    public void AddAbilityUse(TimeSpan useTime)
    {
        if (!this.timeline.ContainsKey(useTime))
        {
            throw new ArgumentException($"The timeline does not support abilities used at time {useTime}.");
        }

        if (this.timeline[useTime] != JobAbilityStatus.Available)
        {
            throw new ArgumentException($"Job ability {this.ability.Name} is not available to use at time {useTime}. It's current status is {this.timeline[useTime]}");
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

    public bool IsCastingAt(TimeSpan time)
    {
        return this.abilityUses.Any(use => use.CastTime == time);
    }

    public JobAbilityStatus GetAbilityStatus(TimeSpan time)
    {
        if (!this.timeline.ContainsKey(time))
        {
            throw new ArgumentException($"The timeline does not support abilities used at time {time}.");
        }

        return this.timeline[time];
    }

    private void UpdateAbilityTimeline()
    {
        foreach (TimeSpan second in this.timeline.Keys)
        {
            JobAbilityStatus status = JobAbilityStatus.Available;

            TimeSpan? mostRecentUse = this.abilityUses.LastOrDefault(use => use.CastTime <= second)?.CastTime;

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
                TimeSpan? nextUse = this.abilityUses.FirstOrDefault(use => use.CastTime > second)?.CastTime;

                if (nextUse.HasValue && second + this.ability.Cooldown > nextUse.Value)
                {
                    status = JobAbilityStatus.Unavailable;
                }
            }

            this.timeline[second] = status;
        }
    }
}