using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.ComponentModel;

public class BlobTimelineSaver
{
    private BlobServiceClient client;
    private readonly IOptions<BlobTimelineSaverOptions> options;

    public BlobTimelineSaver(BlobServiceClient client, IOptions<BlobTimelineSaverOptions> options)
    {
        if (client == null)
        {
            throw new ArgumentNullException(nameof(client));
        }

        this.client = client;
        this.options = options;
    }

    public async Task SaveTimelineAsync(MitigationTimeline timeline)
    {
        string timelineJson = JsonConvert.SerializeObject(timeline);
        BlobContainerClient containerClient = this.client.GetBlobContainerClient(this.options.Value.ContainerName);

        await containerClient.CreateIfNotExistsAsync();

        BlobClient blobClient = containerClient.GetBlobClient(timeline.Id.ToString());
        await blobClient.UploadAsync(BinaryData.FromString(timelineJson), overwrite: true);
    }

    public async Task<MitigationTimeline> LoadTimelineAsync(Guid id)
    {
        BlobContainerClient containerClient = this.client.GetBlobContainerClient(this.options.Value.ContainerName);
        BlobClient blobClient = containerClient.GetBlobClient(id.ToString());

        BlobDownloadResult result = await blobClient.DownloadContentAsync();
        string serializedContent = result.Content.ToString();
        return JsonConvert.DeserializeObject<MitigationTimeline>(serializedContent);
    }

    public async Task<bool> TimelineExistsAsync(Guid id)
    {
        BlobContainerClient containerClient = this.client.GetBlobContainerClient(this.options.Value.ContainerName);
        BlobClient blobClient = containerClient.GetBlobClient(id.ToString());

        Azure.Response<bool> result = await blobClient.ExistsAsync();
        return result.Value;
    }
}

public class BlobTimelineSaverOptions
{
    public Uri BlobUri { get; set; }

    public string ContainerName { get; set; }
}