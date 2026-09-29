using MediatR;

namespace ArticlesApp.Application.Common.Events
{
    public record CacheInvalidationEvent(IReadOnlyCollection<string> Tags) : INotification;
}
