using System.Security.Claims;
using Application.Abstractions;

namespace ArticlesAPI.Identity
{
    public sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

        public Guid? UserId => User?.GetUserId();

        public string? UserName => User?.FindFirst(ClaimTypes.Name)?.Value;

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
    }
}
