using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;

public class BossDataProvider 
{    
    private readonly HttpClient client;

    public BossDataProvider(HttpClient client)
    {
        if (client == null) throw new ArgumentNullException(nameof(client));

        this.client = client;
    }

    private Dictionary<string, string> NameToFileMap = new Dictionary<string, string>
    {
        ["P1S"] = "boss-data/p1s.json"
    };

    public IEnumerable<string> GetBosses()
    {
        return this.NameToFileMap.Keys;
    }

    public Task<BossTimeline> GetBossTimelineAsync(string bossName)
    {
        if (string.IsNullOrEmpty(bossName))
        {
            throw new ArgumentNullException(nameof(bossName));
        }

        if (!this.NameToFileMap.ContainsKey(bossName))
        {
            throw new ArgumentException($"Boss name {bossName} is not supported.");
        }

        return this.client.GetFromJsonAsync<BossTimeline>(this.NameToFileMap[bossName]);
    }
}
