using App.Core.Auth.Models;

namespace App.Core.Auth.Identity.Interface
{
    public interface IJwtProvider
    {
        string GenerateJwtToken(ApplicationUser user, IList<string> roles);
    }
}
