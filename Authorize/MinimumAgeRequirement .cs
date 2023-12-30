using Microsoft.AspNetCore.Authorization;

namespace demoAsp2.Authorize
{

    ///1 requiment có thể cho nhiều handler xử lí: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-6.0
    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public int MinimumAge { get; }
        public MinimumAgeRequirement(int minimumAge)
        {
            MinimumAge = minimumAge;
        }





    }
}
