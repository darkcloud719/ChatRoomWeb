using System.Security.Claims;

namespace ChatRoomWeb.Infrastructure
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetUserId(this ClaimsPrincipal @this)
        {
            return @this.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}