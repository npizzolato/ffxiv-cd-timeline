using Newtonsoft.Json;

// Represents the state of all the mitigation for a single fight.
public class MitigationTimeline
{
    private readonly TimeSpan lastAbilityTimeline;
    private Dictionary<string, Dictionary<string, AbilityTimeline>> jobAbilityTimelines { get; } = 
        new Dictionary<string, Dictionary<string, AbilityTimeline>>();

    public MitigationTimeline(Guid id, BossTimeline bossTimeline)
    {
        this.Id = id;
        this.BossTimeline = bossTimeline;
        this.lastAbilityTimeline = bossTimeline.Timeline.Last().EffectTime;
    }

    [JsonConstructor]
    public MitigationTimeline(Guid id, BossTimeline bossTimeline, IEnumerable<JobInstance> jobs)
    {
        this.Id = id;
        this.BossTimeline = bossTimeline;
        this.lastAbilityTimeline = bossTimeline.Timeline.Last().EffectTime;

        foreach (JobInstance job in jobs)
        {
            this.AddJobInstance(job);
        }
    }

    public Guid Id { get; }

    public BossTimeline BossTimeline { get; }

    public List<JobInstance> Jobs { get; } = new List<JobInstance>();

    public Dictionary<string, Dictionary<string, AbilityTimeline>> JobAbilityTimelines => this.jobAbilityTimelines;

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

    public void AddJobInstance(JobInstance jobInstance)
    {
        this.Jobs.Add(jobInstance);

        Dictionary<string, AbilityTimeline> jobAbilityTimeline = new Dictionary<string, AbilityTimeline>();
        
        foreach (JobAbility ability in jobInstance.JobData.Abilities)
        {
            AbilityTimeline abilityTimeline = new AbilityTimeline(this.lastAbilityTimeline, ability);
            jobAbilityTimeline[ability.Name] = abilityTimeline;
        }

        this.jobAbilityTimelines.Add(jobInstance.Id, jobAbilityTimeline);
    }
    
    public void RemoveJob(JobInstance job)
    {
        this.Jobs.Remove(job);
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