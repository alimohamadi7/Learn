using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;


namespace Learn.Core.Security
{
  public static class GetClaimDeny
    {
        public static string GetEmail(this IIdentity identity )
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.Email);

            return claim?.Value ?? string.Empty;
        }

        public static List<int> GetActor(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
          List<Claim> claim = claimsIdentity?.FindAll(ClaimTypes.Actor).ToList();
            List<int> claimint = new List<int>();
            foreach (var item in claim)
            { 
                int val = Convert.ToInt32(item.Value);


                claimint.Add(val);
            }
            return claimint;
        }
        public static List<int> GetRole(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            List<Claim> claim = claimsIdentity?.FindAll(ClaimTypes.Role).ToList();
            List<int> Roleclaim = new List<int>();
            foreach (var item in claim)
            {
                int val = Convert.ToInt32(item.Value);


                Roleclaim.Add(val);
            }
            return Roleclaim;
        }
    }
}
