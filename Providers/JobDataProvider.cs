using Microsoft.Extensions.Options;
using Newtonsoft.Json;

public class JobDataProvider 
{
    private readonly IOptions<JobDataProviderOptions> options;

    public JobDataProvider(IOptions<JobDataProviderOptions> options)
    {
        if (options == null) { throw new ArgumentNullException(nameof(options)); }

        this.options = options;

        Console.WriteLine($"Loaded {options.Value.JobDataFileMaps.Count} levels.");
    }


    public IEnumerable<string> GetJobs(int level)
    {
        Console.WriteLine($"Loaded {options.Value.JobDataFileMaps.Count} levels.");
        if (!this.options.Value.JobDataFileMaps.ContainsKey(level.ToString()))
        {
            return new List<string>();
        }
        else 
        {
            return this.options.Value.JobDataFileMaps[level.ToString()].Keys;
        }
    }

    public async Task<Job> GetJobDataAsync(string jobName, int level)
    {
        if (string.IsNullOrEmpty(jobName))
        {
            throw new ArgumentNullException(nameof(jobName));
        }

        if (!this.options.Value.JobDataFileMaps.ContainsKey(level.ToString()))
        {
            throw new ArgumentException($"Level {level} is not supported.");
        }

        if (!this.options.Value.JobDataFileMaps[level.ToString()].ContainsKey(jobName))
        {
            throw new ArgumentException($"Job {jobName} is not supported at level {level}.");
        }

        string content = await File.ReadAllTextAsync(this.options.Value.JobDataFileMaps[level.ToString()][jobName]);
        return JsonConvert.DeserializeObject<Job>(content);
    }
}

// This should be its own file but dotnet isn't finding it when it's a file under Providers... 
public class JobDataProviderOptions
{
    public Dictionary<string, Dictionary<string, string>> JobDataFileMaps { get; set; }
}