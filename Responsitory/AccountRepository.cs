using demoAsp2.Data;
using demoAsp2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace demoAsp2.Responsitory
{
    public class AccountRepository : IAccountRepository
    {
        //https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.signinmanager-1?view=aspnetcore-6.0
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;

        private readonly RoleManager<IdentityRole> roleManager;
        private readonly DBAspDemo2Context context;
        private readonly IUrlHelperFactory urlHelper;


        public AccountRepository(
            UserManager<ApplicationUser> userManager,
                 IConfiguration configuration
            , SignInManager<ApplicationUser> signInManager,

            RoleManager<IdentityRole> roleManager,
            DBAspDemo2Context context,
            IUrlHelperFactory urlHelper





            )
        {

            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;

            this.roleManager = roleManager;
            this.context = context;
            this.urlHelper = urlHelper;


        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {


            var user = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email

            };



            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {









                ///tao claim


                var roleClaims = new List<Claim> {

                  new Claim("Customer","canCreate"),
                  new Claim("Customer","canRead"),
                  new Claim("Customer","canUpdate"),
                  new Claim("Customer","canDelete"),


                 };








                ///kiem tra roll da có chưa , nếu chưa thi tạo roll(nên seed data)
                /*
                 if (!await roleManager.RoleExistsAsync(AppRole.Customer))
                 {///chen vao bảng AspNetRoles : 
                     await roleManager.CreateAsync(new IdentityRole(AppRole.Customer));

                     var existingRole = await roleManager.FindByNameAsync(AppRole.Customer);

                     foreach (var claim in roleClaims)
                     {
                         await roleManager.AddClaimAsync(existingRole, claim);
                     }
                 }



                 //chèn vào bảng trung gian là NetuserRoles
                 await userManager.AddToRoleAsync(user, AppRole.Customer);


                 ///nen thêm vào claim ở bảng user claim để khỏi mât công tạo ở dưới

             */


            }

            return result;
        }

        public async Task<string> SignInAsync(SignInModel model)
        {


            var user = await userManager.FindByEmailAsync(model.Email);
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);


            if (!result.Succeeded)
            {
                return "";
            }

            ///tao refresh token:Có thể trả về refresh token lúc đăng nhập cùng vs jwt

            var exist_RefresToken = context.refreshToken.FirstOrDefault(rft => rft.UserId == user.Id);


            if (exist_RefresToken != null)
            {
                exist_RefresToken.Token = GenerateRefreshToken();
                exist_RefresToken.Expires = DateTime.Now.AddDays(20);
                exist_RefresToken.CreateTime = DateTime.Now;


                context.refreshToken.Update(exist_RefresToken);
                Console.WriteLine("token da ton tai dang cap nhat token moi");
                context.SaveChanges();

            }
            else
            {
                context.refreshToken.Add(GenerateUserRefreshToken(user.Id));
                Console.WriteLine("đã tạo 1 refresh token mới");
                context.SaveChanges();
            }




            ///claim này nên tạo lúc đăng kí rồi thêm vào user claim sau đó chỉ cần lấy ra
            var authClaims = new List<Claim> {

                new Claim(ClaimTypes.Email, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("age","23"),
                new Claim("sex","female"),
                new Claim("userId",user.Id)



            };



            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {

                authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));


            }


            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                (configuration["JWT:Serect"])
            );
            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(500),
                claims: authClaims,

                /*

                 claims: authClaims: Tương ứng với 
                "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress": "luthevinh@gmail.com" và 
                "jti": "a4632acd-ed43-4589-bfd9-e6b2d8ce8126".
                 */

                signingCredentials: new SigningCredentials(

                    authenKey, SecurityAlgorithms.HmacSha512
                    )

              );


            return new JwtSecurityTokenHandler().WriteToken(token);

        }


        public string GenerateRefreshToken()
        { ///tạo ra vùng chứa kiểu byte 40 so 
            var randomNumber = new byte[40];
            using var rng = RandomNumberGenerator.Create();


            //tao random chuỗi
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }


        public UserRefreshToken GenerateUserRefreshToken(string userId)
        {

            var userRefreshToken = new UserRefreshToken();

            userRefreshToken.UserId = userId;
            userRefreshToken.Expires = DateTime.Now.AddDays(20);
            userRefreshToken.CreateTime = DateTime.Now;
            userRefreshToken.Token = GenerateRefreshToken();

            return userRefreshToken;

        }




    }
}
