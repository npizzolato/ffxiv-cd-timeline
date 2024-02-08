
namespace FfxivCdTimeline_Server.Providers
{
    public class NoOpTimelineSaver : ITimelineSaver
    {
        public Task<MitigationTimeline> LoadTimelineAsync(Guid id)
        {
            return Task.FromResult(new MitigationTimeline(Guid.NewGuid(), null!));
        }

        public Task SaveTimelineAsync(MitigationTimeline timeline)
        {
            return Task.CompletedTask;
        }

        public Task<bool> TimelineExistsAsync(Guid id)
        {
            return Task.FromResult(false);
        }
    }
}
