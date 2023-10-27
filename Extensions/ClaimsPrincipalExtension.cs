using System.Security.Claims;

namespace RunnerUp.Extensions;

public static class ClaimsPrincipalExtension
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}