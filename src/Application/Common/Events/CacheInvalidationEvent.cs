using MediatR;

namespace Application.Common.Events
{
    public record CacheInvalidationEvent(HashSet<string> Tags) : INotification;
}
