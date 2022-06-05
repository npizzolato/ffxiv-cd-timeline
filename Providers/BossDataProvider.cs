using Newtonsoft.Json;

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
        ["P1S"] = "boss-data/p1s.json",
        ["UWU"] = "boss-data/uwu.json"
    };

    public IEnumerable<string> GetBosses()
    {
        return this.NameToFileMap.Keys;
    }

    public async Task<BossTimeline> GetBossTimelineAsync(string bossName)
    {
        if (string.IsNullOrEmpty(bossName))
        {
            throw new ArgumentNullException(nameof(bossName));
        }

        if (!this.NameToFileMap.ContainsKey(bossName))
        {
            throw new ArgumentException($"Boss name {bossName} is not supported.");
        }

        HttpResponseMessage response = await this.client.GetAsync(this.NameToFileMap[bossName]);
        string content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BossTimeline>(content);
    }
}
