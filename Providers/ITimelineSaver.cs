public interface ITimelineSaver
{
    Task SaveTimelineAsync(MitigationTimeline timeline);

    Task<MitigationTimeline> LoadTimelineAsync(Guid id);

    Task<bool> TimelineExistsAsync(Guid id);
}