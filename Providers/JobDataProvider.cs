using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        ["Astrologian"] = "job-data/astrologian.json",
        ["Dark Knight"] = "job-data/darkknight.json",
        ["Gunbreaker"] = "job-data/gunbreaker.json",
        ["Paladin"] = "job-data/paladin.json",
        ["Sage"] = "job-data/sage.json",
        ["Scholar"] = "job-data/scholar.json",
        ["Warrior"] = "job-data/warrior.json",
        ["White Mage"] = "job-data/whitemage.json"
    };

    public IEnumerable<string> GetJobs()
    {
        return this.NameToFileMap.Keys;
    }

    public async Task<Job> GetJobDataAsync(string jobName)
    {
        if (string.IsNullOrEmpty(jobName))
        {
            throw new ArgumentNullException(nameof(jobName));
        }

        if (!this.NameToFileMap.ContainsKey(jobName))
        {
            throw new ArgumentException($"Job name {jobName} is not supported.");
        }

        HttpResponseMessage response = await this.client.GetAsync(this.NameToFileMap[jobName]);
        string content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Job>(content);
    }
}
