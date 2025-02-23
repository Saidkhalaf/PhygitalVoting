using System.ComponentModel.DataAnnotations;
using PM.BL.Domain.subthemes;
using PM.BL.subthemeManager;
using PM.DAL.EF.subthemeRepository;

namespace PM.BL.SubthemeManager;

public class SubthemeManager : ISubthemeManager
{
    private readonly ISubthemeRepository _repo;

    public SubthemeManager(ISubthemeRepository repo)
    {
        _repo = repo;
    }
    
    public IEnumerable<Subtheme> GetAllSubthemes()
    {
        return _repo.ReadAllSubthemes();
    }

    public Subtheme AddSubtheme(string name, string description, string image, int flowId)
    {
        Subtheme subtheme = new Subtheme
        {
            Name = name,
            Description = description,
            Image = image,
            FlowId = flowId
        };
        
        ValidateSubtheme(subtheme);

        _repo.CreateSubtheme(subtheme);
        return subtheme;
    }
    
    public Subtheme GetSubthemeById(int id)
    {
        return _repo.ReadSubthemeById(id);
    }

    public int GetTotalSubthemes()
    {
        return _repo.ReadTotalSubthemes();
    }

    public void UpdateSubtheme(Subtheme subtheme)
    {
        _repo.UpdateSubtheme(subtheme);
    }

    public void RemoveSubtheme(int subthemeId)
    {
        _repo.DeleteSubtheme(subthemeId);
    }

    private void ValidateSubtheme(Subtheme subtheme)
    {
        ValidationContext vc = new ValidationContext(subtheme);
        List<ValidationResult> result = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(subtheme, vc, result, validateAllProperties: true);

        if (!isValid)
        {
            string errorMessage = "";
            foreach (ValidationResult vr in result)
            {
                errorMessage += vr.ErrorMessage + "\n";
            }

            throw new ValidationException(errorMessage);
        }
    }
}