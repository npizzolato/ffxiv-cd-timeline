public class BossTimeline 
{
    public string ShortName { get; set; }

    public string FullName { get; set; }

    public List<BossAbility> Abilities { get; set; }

    public List<BossTimelineEntry> Timeline { get; set; }
}