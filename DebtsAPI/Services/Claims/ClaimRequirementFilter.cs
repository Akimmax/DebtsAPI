using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DebtsAPI.Services.Claims
{
    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly Claim _claim;

        public ClaimRequirementFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string targetId = null; 

            switch (_claim.ValueType)
            {
                case ClaimValueSources.FROM_PATH:
                    targetId = context.RouteData.Values[_claim.Value] as string;
                    break;
            }

            var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == _claim.Type && c.Value == targetId);
            if (!hasClaim)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
