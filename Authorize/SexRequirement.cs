using Microsoft.AspNetCore.Authorization;

namespace demoAsp2.Authorize
{
    public class SexRequirement : IAuthorizationRequirement
    {

        public string RequiredSex { get; }

        public SexRequirement(string requiredSex)
        {
            RequiredSex = requiredSex;
        }
    }
}
