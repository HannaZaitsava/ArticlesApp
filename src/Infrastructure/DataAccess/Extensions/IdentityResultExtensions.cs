using Microsoft.AspNetCore.Identity;

namespace ArticlesApp.Infrastructure.DataAccess.Extensions
{
    public static class IdentityResultExtensions
    {        
        public static string ToErrorString(this IdentityResult result)
        {
            return string.Join(" | ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
        }
    }
}
