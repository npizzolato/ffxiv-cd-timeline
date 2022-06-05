using Microsoft.Extensions.Options;
using Newtonsoft.Json;

public class BossDataProvider 
{    
    private readonly HttpClient client;
    private readonly IOptions<BossDataProviderOptions> options;

    public BossDataProvider(HttpClient client, IOptions<BossDataProviderOptions> options)
    {
        if (client == null) throw new ArgumentNullException(nameof(client));
        if (options == null) throw new ArgumentNullException(nameof(options));

        this.client = client;
        this.options = options;
    }

    public IEnumerable<BossMetadata> GetBosses()
    {
        Console.WriteLine($"Loading {this.options.Value.Bosses.Count()} bosses.");
        return this.options.Value.Bosses;
    }

    public async Task<BossTimeline> GetBossTimelineAsync(string bossName)
    {
        if (string.IsNullOrEmpty(bossName))
        {
            throw new ArgumentNullException(nameof(bossName));
        }
        
        BossMetadata metadata = this.options.Value.Bosses.FirstOrDefault(b => b.ShortName.Equals(bossName));

        if (metadata == null)
        {
            throw new ArgumentException($"Boss name {bossName} is not supported.");
        }

        HttpResponseMessage response = await this.client.GetAsync(metadata.DetailsFile);
        string content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<BossTimeline>(content);
    }
}

public class BossDataProviderOptions
{
    public List<BossMetadata> Bosses { get; set; }
}
