using Microsoft.AspNetCore.Identity;

namespace PM.DAL.EF.subPlatformAdminRepository;

public interface ISubPlatformAdminRepository
{
    IdentityUser ReadUser(string id);
}