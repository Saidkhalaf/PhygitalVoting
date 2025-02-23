using System.ComponentModel.DataAnnotations;
using PM.BL.Domain.projects;
using PM.BL.Domain.subthemes;

namespace PM.BL.Domain.flows;

public class Flow
{
    [Key] public int Id { get; set; }
    [MinLength(1)] public string Name { get; set; }
    public ICollection<Subtheme> Subthemes { get; set; }
    public FlowType FlowType { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }
    public Flow()
    {
    }

    public Flow(int id, string name, FlowType flowType, int projectId, Project project)
    {
        Id = id;
        Name = name;
        FlowType = flowType;
        ProjectId = projectId;
        Project = project;
        Subthemes = new List<Subtheme>();
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        if (string.IsNullOrEmpty(Name) || Name.Trim().Length < 2)
        {
            results.Add(new ValidationResult("Name should be at least 2 characters long.", new[] { nameof(Name) }));
        }

        if (Subthemes.Count < 1)
        {
            results.Add(new ValidationResult("Flow should contain at least one subtheme.",
                new[] { nameof(Subthemes) }));
        }

        return results;
    }
}