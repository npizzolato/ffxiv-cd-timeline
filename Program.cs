using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using FfxivCdTimeline;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var options = new JobDataProviderOptions();
builder.Configuration.GetSection("jobDataProviderOptions").Bind(options);
Console.WriteLine($"Startup: {builder.Configuration.GetDebugView()}");
Console.WriteLine($"Startup: Loaded {options.JobDataFileMaps.Count} levels.");

builder.Services.Configure<JobDataProviderOptions>(builder.Configuration.GetSection("jobDataProviderOptions"));

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<BossDataProvider>();
builder.Services.AddScoped<JobDataProvider>();
builder.Services.AddScoped<TimelineSaver>();

await builder.Build().RunAsync();
