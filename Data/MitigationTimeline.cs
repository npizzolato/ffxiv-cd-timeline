// Represents the state of all the mitigation for a single fight.
public class MitigationTimeline
{
    private readonly TimeSpan lastAbilityTimeline;
    private Dictionary<string, Dictionary<string, AbilityTimeline>> jobAbilityTimelines { get; } = 
        new Dictionary<string, Dictionary<string, AbilityTimeline>>();

    public MitigationTimeline(BossTimeline bossTimeline)
    {
        this.BossTimeline = bossTimeline;
        this.lastAbilityTimeline = bossTimeline.Timeline.Last().EffectTime;
    }

    public BossTimeline BossTimeline { get; }

    public List<JobInstance> Jobs { get; } = new List<JobInstance>();

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

        this.jobAbilityTimelines.Add(jobInstance.Id, jobAbilityTimeline);

        return jobInstance;
    }

    public AbilityTimeline GetAbilityTimeline(JobInstance job, string abilityName)
    {
        return this.jobAbilityTimelines[job.Id][abilityName];
    }

    public IEnumerable<TimeSpan> GetTimelineRange()
    {
        return Enumerable.Range(0, Convert.ToInt32(this.BossTimeline.Timeline.Last().EffectTime.TotalSeconds + 1))
            .Select(multiplier => TimeSpan.Zero.Add(TimeSpan.FromSeconds(1 * multiplier)));
    }

    public IEnumerable<TimeSpan> GetImportantTimelinePoints()
    {
        IEnumerable<TimeSpan> bossCastTimes = this.BossTimeline.Timeline.Select(e => e.EffectTime);

        HashSet<TimeSpan> abilityCastTimes = new HashSet<TimeSpan>();

        foreach (KeyValuePair<string, Dictionary<string, AbilityTimeline>> jobTimeline in this.jobAbilityTimelines)
        {
            foreach (AbilityTimeline abilityTimeline in jobTimeline.Value.Values)
            {
                foreach (TimeSpan castTime in abilityTimeline.GetCastTimes())
                {
                    abilityCastTimes.Add(castTime);
                }
            }
        }

        return abilityCastTimes.Union(bossCastTimes).OrderBy(t => t);
    }
}