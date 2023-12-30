using demoAsp2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Security.Claims;

namespace demoAsp2.Authorize
{
    public class crudHandlers : IAuthorizationHandler
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly DBAspDemo2Context context;

        public crudHandlers(RoleManager<IdentityRole> roleManager, DBAspDemo2Context context)
        {
            this.roleManager = roleManager;
            this.context = context;

        }

        public async Task HandleAsync(
           AuthorizationHandlerContext context


            )


        {




            var pendingRequirements = context.PendingRequirements.ToList();

            //context.Fail();

            foreach (var requirement in pendingRequirements)
            {
                if (requirement is permissonCrud)
                {

                    var crudRequirement = requirement as permissonCrud;

                    if (await isHasRoleClaim(crudRequirement, context, roleManager))
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }











                }


            }





            await Task.CompletedTask;
        }

        private async Task<bool> isHasRoleClaim(permissonCrud? crudRequirement, AuthorizationHandlerContext context, RoleManager<IdentityRole> roleManager)
        {

            bool existModuleAndAction = false;

            //context.Succeed(crudRequirement);

            var ListRolesClaim = context.User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

            var roleDBs = roleManager.Roles.Where(role => ListRolesClaim.Any(roleClaim => roleClaim == role.Name)).ToList();







            foreach (var roleDB in roleDBs)
            {
                var role = await roleManager.FindByNameAsync(roleDB.Name);
                //Console.WriteLine($"role---" + roleDB.Id);

                if (role != null)
                {
                    Console.WriteLine($"da lay dc claim-------------");
                    var roleClaims = await roleManager.GetClaimsAsync(role);

                    existModuleAndAction = roleClaims.Any(rcl => rcl.Type == crudRequirement?.Module
                    && rcl.Value == crudRequirement?.Action);





                }


            }


            return existModuleAndAction;

        }


    }
}
