using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using PM.BL.Domain.flows;

namespace PM.BL.Domain.projects;

public class Project
{
    [Key]
    public int Id { get; set; }
    [MinLength(1)]
    public string Name { get; set; }
    public bool Status { get; set; }

    public int ParticipantCount { get; set; }
    public ICollection<Flow> Flows { get; set; }
    [Required]
    public IdentityUser ManagerUser { get; set; }
    
    public Project()
    {
    }
    
    public Project(int id, string name, bool status, ICollection<Flow> flows)
    {
        Id = id;
        Name = name;
        Status = status;
        Flows = flows;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    { 
        var results = new List<ValidationResult>();

        if (string.IsNullOrEmpty(Name) || Name.Trim().Length < 2)
        {
            results.Add(new ValidationResult("Name should be at least 2 characters long.", new[] { nameof(Name) }));
        }
        
        if (Flows.Count < 1)
        {
            results.Add(new ValidationResult("Project should contain at least one flow.", new[] { nameof(Flows) }));
        }
        
        return results;
    }
}