// Represents the timeline for ability use by a boss/enemy.
public class BossTimeline 
{
    public string ShortName { get; set; }

    public string FullName { get; set; }

    public int Level { get; set; }

    public List<BossAbility> Abilities { get; set; }

    public List<BossTimelineEntry> Timeline { get; set; }
}