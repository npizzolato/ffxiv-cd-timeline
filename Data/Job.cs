public class Job
{
    public string Name { get; set; }

    public List<JobAbility> Abilities { get; set; } = new List<JobAbility>();

    public List<JobTimelineEntry> AbilityTimeline { get; set; } = new List<JobTimelineEntry>();
}
