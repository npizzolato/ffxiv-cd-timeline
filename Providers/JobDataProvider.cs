using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;

public class JobDataProvider 
{    
    private readonly HttpClient client;

    public JobDataProvider(HttpClient client)
    {
        if (client == null) throw new ArgumentNullException(nameof(client));

        this.client = client;
    }

    private Dictionary<string, string> NameToFileMap = new Dictionary<string, string>
    {
        ["warrior"] = "job-data/warrior.json"
    };

    public IEnumerable<string> GetJobs()
    {
        return this.NameToFileMap.Keys;
    }

    public Task<Job> GetJobDataAsync(string jobName)
    {
        if (string.IsNullOrEmpty(jobName))
        {
            throw new ArgumentNullException(nameof(jobName));
        }

        if (!this.NameToFileMap.ContainsKey(jobName))
        {
            throw new ArgumentException($"Job name {jobName} is not supported.");
        }

        return this.client.GetFromJsonAsync<Job>(this.NameToFileMap[jobName]);
    }
}