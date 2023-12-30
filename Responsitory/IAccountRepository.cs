using demoAsp2.Models;
using Microsoft.AspNetCore.Identity;

namespace demoAsp2.Responsitory
{
    public interface IAccountRepository
    {

        public Task<IdentityResult> SignUpAsync(SignUpModel model);
        public Task<string> SignInAsync(SignInModel model);

        public UserRefreshToken GenerateUserRefreshToken(string userId);


        public string GenerateRefreshToken();

    }
}
