using Microsoft.AspNetCore.Identity;

namespace UserLogManagement.Models
{
    public class Applicationuser : IdentityUser
    {
        public string? Name { get; set; }

        public string? Mobile { get; set; }

        public string? Gender { get; set; }
    }
}
