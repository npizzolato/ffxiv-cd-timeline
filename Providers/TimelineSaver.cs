using Newtonsoft.Json;

public class TimelineSaver
{
    public string SaveTimeline(MitigationTimeline timeline)
    {
        string timelineJson = JsonConvert.SerializeObject(timeline);
        byte[] data = System.Text.Encoding.UTF8.GetBytes(timelineJson);
        return System.Convert.ToBase64String(data);
    }

    public MitigationTimeline LoadTimeline(string savedTimeline)
    {
        byte[] data = System.Convert.FromBase64String(savedTimeline);
        string base64Decoded = System.Text.ASCIIEncoding.ASCII.GetString(data);
        return JsonConvert.DeserializeObject<MitigationTimeline>(base64Decoded);
    }
}