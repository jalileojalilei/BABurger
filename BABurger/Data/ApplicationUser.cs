using Microsoft.AspNetCore.Identity;

namespace BABurger.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
