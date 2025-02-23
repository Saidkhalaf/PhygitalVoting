using PM.BL.Domain.subthemes;

namespace PM.BL.subthemeManager;

public interface ISubthemeManager
{
    public Subtheme AddSubtheme(string name, string description, string image, int flowId);
    void RemoveSubtheme(int subthemeId);
    int GetTotalSubthemes();
    void UpdateSubtheme(Subtheme subtheme);
    Subtheme GetSubthemeById(int id);
}