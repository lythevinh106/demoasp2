using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace demoAsp2.Authorize
{
    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        //https://learn.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-6.0
        protected override Task HandleRequirementAsync

            (AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {

            var ageClaim = context.User.Claims.FirstOrDefault(c => c.Type == "age");
            //Console.WriteLine("claim age -----" + ageClaim?.Value);


            var roleClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            Console.WriteLine("claim role -----" + roleClaim?.Value);



            if (ageClaim == null || Int32.Parse(ageClaim.Value) <= requirement.MinimumAge)
            {


                context.Fail();
                /*sẽ k dừng chương trình nhưng nếu ở cuối là 
                         context.Fail(); thì nó sẽ dừng
                 
                 */
            }
            else
            {
                /// đánh giá răng yêu cầu đc đáp ứng
                context.Succeed(requirement);
            }







            return Task.CompletedTask;
        }
    }
}
