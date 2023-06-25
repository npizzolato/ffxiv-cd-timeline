using Microsoft.Extensions.Options;
using Newtonsoft.Json;

public class BossDataProvider 
{    
    private readonly IOptions<BossDataProviderOptions> options;

    public BossDataProvider(IOptions<BossDataProviderOptions> options)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));

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

        string content = await File.ReadAllTextAsync(metadata.DetailsFile);
        return JsonConvert.DeserializeObject<BossTimeline>(content);
    }
}

public class BossDataProviderOptions
{
    public List<BossMetadata> Bosses { get; set; }
}
