using Microsoft.AspNetCore.Identity;

namespace PM.DAL.EF.subPlatformAdminRepository;

public class SubPlatformAdminRepository : ISubPlatformAdminRepository
{
    private readonly PhygitalDbContext _context;
    
    public SubPlatformAdminRepository(PhygitalDbContext context)
    {
        _context = context;
    }

    public IdentityUser ReadUser(string id)
    {
        return _context.Users.Single(user => user.Id == id);
    }
}