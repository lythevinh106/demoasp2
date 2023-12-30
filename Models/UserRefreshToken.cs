using demoAsp2.Data;

namespace demoAsp2.Models
{
    public class UserRefreshToken
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; }

        public virtual ApplicationUser user { get; set; }



    }
}
