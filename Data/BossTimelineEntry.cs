// Represnts a single entry in the timeline for a boss's ability use.
public class BossTimelineEntry
{
    public string Name { get; set; }

    public TimeSpan? CastTime { get; set; }

    public TimeSpan EffectTime { get; set; }
}