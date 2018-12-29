using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DebtsAPI.Services.Claims
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(string claimType, string claimValue, string claimValueType) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue, claimValueType) };
        }
    }

}
