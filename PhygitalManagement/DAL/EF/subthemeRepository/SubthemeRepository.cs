using Microsoft.EntityFrameworkCore;
using PM.BL.Domain.subthemes;

namespace PM.DAL.EF.subthemeRepository;

public class SubthemeRepository : ISubthemeRepository
{
    private readonly PhygitalDbContext _context;

    public SubthemeRepository(PhygitalDbContext context)
    {
        _context = context;
    }


    public IEnumerable<Subtheme> ReadAllSubthemes()
    {
        return _context.Subthemes.AsEnumerable();
    }


    public void UpdateSubtheme(Subtheme subtheme)
    {
        _context.Update(subtheme);
        _context.SaveChanges();
    }

    public void CreateSubtheme(Subtheme subtheme)
    {
        _context.Subthemes.Add(subtheme);
        _context.SaveChanges();
    }
    
    public void DeleteSubtheme(int subthemeId)
    {
        var subtheme = _context.Subthemes.Single(s => s.Id == subthemeId);
        if (subtheme != null)
        {
            _context.Subthemes.Remove(subtheme);
            _context.SaveChanges();
        }
    }
    
    public int ReadTotalSubthemes()
    {
        return _context.Subthemes.Count();
    }
    
    public Subtheme ReadSubthemeById(int id)
    {
        return _context.Subthemes.Include(qs=>qs.Elements)
            .Single(s => s.Id == id);
    }

}