using MediatR;

namespace ArticlesApp.Application.Common.Events
{
    public record CacheInvalidationEvent(HashSet<string> Tags) : INotification;
}
