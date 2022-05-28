using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;

[Route("api/bossData")]
[Controller]
public class BossDataController : ControllerBase
{
    private readonly HttpClient client;

    public BossDataController(HttpClient client)
    {
        if (client == null) throw new ArgumentNullException(nameof(client));

        this.client = client;
    }
    private Dictionary<string, string> NameToFileMap = new Dictionary<string, string>
    {
        ["P1S"] = "BossData/p1s.json"
    };

    [HttpGet]
    public ActionResult<IEnumerable<string>> GetBosses()
    {
        return new OkObjectResult(this.NameToFileMap.Keys);
    }

    [HttpGet]
    [Route("/{bossName}")]
    public async Task<ActionResult<BossTimeline>> GetBossTimeline([FromRoute] string bossName)
    {
        if (string.IsNullOrEmpty(bossName))
        {
            return new BadRequestResult();
        }

        if (!this.NameToFileMap.ContainsKey(bossName))
        {
            return new BadRequestObjectResult($"Boss name {bossName} is not supported.");
        }

        BossTimeline timeline = await this.client.GetFromJsonAsync<BossTimeline>(this.NameToFileMap[bossName]);
        return new OkObjectResult(timeline);
    }
}
