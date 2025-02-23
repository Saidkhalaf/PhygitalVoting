using PM.BL.Domain.subthemes;

namespace PM.DAL.EF.subthemeRepository;

public interface ISubthemeRepository
{
    IEnumerable<Subtheme> ReadAllSubthemes();
    void UpdateSubtheme(Subtheme subtheme);
    void DeleteSubtheme(int subthemeId);
    void CreateSubtheme(Subtheme subtheme);
    int ReadTotalSubthemes();
    Subtheme ReadSubthemeById(int id);
}