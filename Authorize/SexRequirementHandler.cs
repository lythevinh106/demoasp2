using Microsoft.AspNetCore.Authorization;

namespace demoAsp2.Authorize
{
    public class SexRequirementHandler : AuthorizationHandler<SexRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SexRequirement requirement)
        {
            // Kiểm tra xem người dùng có giới tính phù hợp không
            if (context.User.HasClaim(c => c.Type == "sex" && c.Value == requirement.RequiredSex))
            {
                Console.WriteLine("kiem tra gioi tinh thanh cong");
                context.Succeed(requirement);




            }
            else
            {
                Console.WriteLine("kiem tra gioi tinh that bai");
                context.Fail();
            }


            return Task.CompletedTask;
        }
    }
}
