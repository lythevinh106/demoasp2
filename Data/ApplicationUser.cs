using demoAsp2.Models;
using Microsoft.AspNetCore.Identity;

namespace demoAsp2.Data
{

    public class ApplicationUser : IdentityUser
    {

        public string FirstName { get; set; } = null!;
        /*
         null!; đảm bảo với trình biên dịch này là giá trị này k bao h null
         nên nó sẽ tắt cảnh bảo null đi
         
         */
        public string LastName { get; set; } = null!;

        public virtual UserRefreshToken refreshToken { get; set; }

        public string? EmailConfirmationToken { get; set; }
    }
}
