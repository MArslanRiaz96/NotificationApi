using System.Security.Claims;

namespace NotificationApi.Extentions
{
    public static class IdentityExtention
    {
        public static string GetUserId(this ClaimsPrincipal claim)
        {
            if (claim == null)
                throw new Exception("User was not parsed from the token");
            var idClaim = claim.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim == null)
                throw new Exception("User Id not found in the Identity");
            return idClaim.Value ?? "";
        }
    }
}
