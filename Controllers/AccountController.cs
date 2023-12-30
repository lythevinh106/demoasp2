using demoAsp2.Data;
using demoAsp2.Dto;
using demoAsp2.Filter;
using demoAsp2.Models;
using demoAsp2.Responsitory;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace demoAsp2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [DemoFilterAction("thangkhovipro@gmail.com")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly DBAspDemo2Context _context;

        private readonly IAppConfig _appConfig;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;

        public AccountController(IAccountRepository accountRepository
        , DBAspDemo2Context context
            , UserManager<ApplicationUser> userManager,






            IConfiguration configuration
            )
        {


            this.userManager = userManager;
            this.configuration = configuration;

            _accountRepository = accountRepository;
            _context = context;







        }

        [HttpPost("SignUp")]


        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {

            var result = await _accountRepository.SignUpAsync(signUpModel);



            if (result.Succeeded)
            {
                var user = await userManager.FindByEmailAsync(signUpModel.Email);

                ///add token verifyEmail

                var tokenVerifyEmail = Guid.NewGuid().ToString() + DateTime.Now.ToString();

                user.EmailConfirmationToken = tokenVerifyEmail;
                _context.SaveChanges();

                var link = Url.Action(nameof(ConfirmEmail), "Account", new { token = tokenVerifyEmail, userId = user.Id }
                , Request.Scheme
                );


                var jobId = BackgroundJob.Enqueue<IMail>(x => x.SendMail($"<a href={link}> bam vao day de xac nhan email cua ban <a>", $"<a href={link}> bam vao day de xac nhan email cua ban <a>",
                               "lythevinh106@gmail.com", user.Email, "56658sa"));


            }
            return Ok();
        }





        [HttpGet("ConfirmEmail/{token}/{userId}")]
        public async Task<IActionResult> ConfirmEmail(string token, string userId)
        {




            var result = _context.Users.Where(u => u.Id == userId && HttpUtility.UrlDecode(token) == u.EmailConfirmationToken).FirstOrDefault();

            if (result != null)
            {

                result.EmailConfirmed = true;
                _context.SaveChanges();
                return Ok("Xac thuc thanh cong");

            }


            return Ok("Xac thuc that bai " + userId + "--" + token)



   ;

        }








        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPass forgetPass)

        {


            var user = await userManager.FindByEmailAsync(forgetPass.Email);

            ///add token verifyEmail

            /*  var tokenVerifyEmail = Guid.NewGuid().ToString() + DateTime.Now.ToString();

              user.EmailConfirmationToken = tokenVerifyEmail;
              _context.SaveChanges();*/


            var tokeResetPassword = await userManager.GeneratePasswordResetTokenAsync(user);




            var link = Url.Action(nameof(ResetPassword), "Account", new { token = tokeResetPassword, id = user.Id }
            , Request.Scheme
            );
            return Ok(tokeResetPassword);

            Console.WriteLine("day la link reset password-----------" + link);

            var jobId = BackgroundJob.Enqueue<IMail>(x => x.SendMail($"<a href={link}> bam vao day reset pass </a>", $"<a href={link}> Nhan vao day {link}</a>",
                           "lythevinh106@gmail.com", user.Email, "56658sa"));





            return Ok();




        }


        //[HttpGet("ResetPassword/{token}/{id}")]

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] Resetpass resetpass)



        {
            string password = "lytheinh@ABC1234";

            var user = await userManager.FindByIdAsync(resetpass.Id);
            try
            {



                Console.WriteLine("day la token ----" + resetpass.Token);

                var result = await userManager.ResetPasswordAsync(user, resetpass.Token, password);



                if (result.Succeeded)
                {
                    return Ok(" thanh cong");

                }
                else
                {
                    foreach (var item in result.Errors.ToList())
                    {
                        Console.WriteLine("error of reset passss-wordd" + item.Description.ToString());

                    }

                    return BadRequest("Reset password failed");
                }


            }

            catch (Exception ex)
            {
                // Xử lý khi có lỗi xảy ra
                // In ra thông tin lỗi để debug

                return StatusCode(500); // Hoặc xử lý lỗi tùy theo yêu cầu
            }



        }








        [HttpPost("SignIn")]


        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {


            var result = await _accountRepository.SignInAsync(signInModel);


            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }

            return Ok(result);


        }

        [Authorize]

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshToken refresToken)

        {


            Console.WriteLine("reffesh token---------" + refresToken.refresToken);

            var claimsPrincipal = HttpContext.User;

            string userIdFromclaims = claimsPrincipal.FindFirstValue("userId");

            if (userIdFromclaims == null)
            {
                return NotFound();
            }




            var exist_RefresToken = _context.refreshToken.FirstOrDefault(rft => rft.UserId == userIdFromclaims

             && rft.Expires > DateTime.Now && refresToken.refresToken == rft.Token

            );


            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userIdFromclaims);

            if (exist_RefresToken != null && user != null)
            {

                exist_RefresToken.Expires = DateTime.Now.AddDays(20);

                _context.SaveChanges();



                var authClaims = new List<Claim> {

                new Claim(ClaimTypes.Email, user.Email),

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
                    expires: DateTime.Now.AddMinutes(20),
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


                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);


                return Ok(jwtToken);





            }
            return BadRequest();
        }


        [Authorize]





        [HttpGet("GetRefreshToken")]

        public async Task<IActionResult> GetRefreshToken()
        {
            var claimsPrincipal = HttpContext.User;

            string userIdFromclaims = claimsPrincipal.FindFirstValue("userId");
            var exist_RefresToken = _context.refreshToken.FirstOrDefault(rft => rft.UserId == userIdFromclaims
            );



            if (exist_RefresToken != null)
            {

                //var refreshToken = _accountRepository.GenerateUserRefreshToken(userIdFromclaims);



                return Ok(exist_RefresToken);



            }


            return BadRequest();
        }
    }
}
