using System.Security.Claims;

namespace WebStoreApi.Services
{
    public class JwtReader
    {
        public static string GetUserId(ClaimsPrincipal User)
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return null;
            }

            var claim = identity.Claims.FirstOrDefault(c => c.Type.ToLower() == "uid");
            if (claim == null)
                return null;

            string id;
            try
            {
                id = claim.Value.ToString();
            }
            catch (Exception)
            {

                return null;
            }
            return id;
        } 
        
        
        public static string GetUserRole(ClaimsPrincipal User)
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return null;
            }

            var claim = identity.Claims.FirstOrDefault(c => c.Type.ToLower().Contains("role"));
            if (claim == null)
                return null;

            
            return claim.Value;
        }
        
        public static Dictionary<string,string> GetUserClaims(ClaimsPrincipal User)
        {
            Dictionary<string, string> claims = new();

            var identity = User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                foreach (Claim c in identity.Claims)
                {
                    claims.Add(c.Type,c.Value);
                    
                }
            }
            return claims;
        }
    }


}
