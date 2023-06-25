using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var options = new JobDataProviderOptions();
builder.Configuration.GetSection("jobDataProviderOptions").Bind(options);
Console.WriteLine($"Startup: {builder.Configuration.GetDebugView()}");
Console.WriteLine($"Startup: Loaded {options.JobDataFileMaps.Count} levels.");

builder.Services.Configure<JobDataProviderOptions>(builder.Configuration.GetSection("jobDataProviderOptions"));
builder.Services.Configure<BossDataProviderOptions>(builder.Configuration.GetSection("bossDataProviderOptions"));
builder.Services.Configure<BlobTimelineSaverOptions>(builder.Configuration.GetSection("blobTimelineSaverOptions"));

builder.Services.AddHttpClient("ApiClient", (provider, client) =>
{
    var manager = provider.GetRequiredService<NavigationManager>();
    client.BaseAddress = new Uri(manager.BaseUri);
});
builder.Services.AddScoped<BossDataProvider>();
builder.Services.AddScoped<JobDataProvider>();

var blobOptions = new BlobTimelineSaverOptions();
builder.Configuration.GetSection("blobTimelineSaverOptions").Bind(blobOptions);
var blobClient = new BlobServiceClient(
    blobOptions.BlobUri,
    new DefaultAzureCredential(new DefaultAzureCredentialOptions
        {
            Diagnostics =
            {
                LoggedHeaderNames = { "x-ms-request-id" },
                LoggedQueryParameters = { "api-version" },
                IsLoggingContentEnabled = true
            }
    }));
builder.Services.AddSingleton<BlobServiceClient>(blobClient);
builder.Services.AddSingleton<BlobTimelineSaver>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
