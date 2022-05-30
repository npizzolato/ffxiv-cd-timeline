public class Timeline
{
    public string Id { get; set; }

    public BossTimeline BossTimeline { get; set; }

    public List<JobInstance> Jobs { get; set; }
}