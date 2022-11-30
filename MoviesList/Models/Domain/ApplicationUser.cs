using Microsoft.AspNetCore.Identity;

namespace MoviesList.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
