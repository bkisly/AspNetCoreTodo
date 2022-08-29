using Microsoft.AspNetCore.Identity;

namespace AspNetCoreTodo.ViewModels
{
    public class ManageUsersViewModel
    {
        public IEnumerable<IdentityUser> Admins { get; set; } = null!;
        public IEnumerable<IdentityUser> Everyone { get; set; } = null!;
    }
}
