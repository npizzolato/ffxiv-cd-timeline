// Represents the state of all the mitigation for a single fight.
public class MitigationTimeline
{
    private readonly TimeSpan lastAbilityTimeline;

    public MitigationTimeline(BossTimeline bossTimeline)
    {
        this.BossTimeline = bossTimeline;
        this.lastAbilityTimeline = bossTimeline.Timeline.Last().EffectTime;
    }

    public BossTimeline BossTimeline { get; }

    public List<JobInstance> Jobs { get; } = new List<JobInstance>();

    public Dictionary<string, Dictionary<string, AbilityTimeline>> JobAbilityTimelines { get; } = new Dictionary<string, Dictionary<string, AbilityTimeline>>();

    public JobInstance AddJob(Job job)
    {
        JobInstance jobInstance = new JobInstance()
        {
            Id = Guid.NewGuid().ToString(),
            JobData = job,
        };

        this.Jobs.Add(jobInstance);

        Dictionary<string, AbilityTimeline> jobAbilityTimeline = new Dictionary<string, AbilityTimeline>();
        
        foreach (JobAbility ability in job.Abilities)
        {
            AbilityTimeline abilityTimeline = new AbilityTimeline(this.lastAbilityTimeline, ability);
            jobAbilityTimeline[ability.Name] = abilityTimeline;
        }

        this.JobAbilityTimelines.Add(jobInstance.Id, jobAbilityTimeline);

        return jobInstance;
    }

    public IEnumerable<TimeSpan> GetTimelineRange()
    {
        return Enumerable.Range(0, Convert.ToInt32(this.BossTimeline.Timeline.Last().EffectTime.TotalSeconds))
            .Select(multiplier => TimeSpan.Zero.Add(TimeSpan.FromSeconds(1 * multiplier)));
    }
}