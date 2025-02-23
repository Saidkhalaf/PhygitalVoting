using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using PM.BL.Domain.elements;
using PM.BL.Domain.flows;
using PM.BL.Domain.questions;

namespace PM.BL.Domain.subthemes;

public class Subtheme 
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [StringLength(150, MinimumLength = 2, ErrorMessage = "At least 2 character, maximum 150 characters")] [Required]
    public string Description { get; set; }

    public string Image { get; set; }
    public ICollection<Element> Elements { get; set; }
    
    public ICollection<Question> Questions { get; set; }
    
    public bool IsSubthemeActive { get; set; }
    public Flow Flow { get; set; }
    public int FlowId { get; set; }
    public bool IsActivated { get; set; }
    
    public Subtheme()
    {
    }
    
    public Subtheme(int id, string name, string description, string image, 
        bool isSubthemeActive, Flow flow)
    {
        Id = id;
        Name = name;
        Description = description;
        Image = image;
        IsSubthemeActive = isSubthemeActive;
        Flow = flow;
        Elements = new List<Element>();
        Questions = new List<Question>();
    }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        if (string.IsNullOrEmpty(Name) || Name.Trim().Length < 2)
        {
            results.Add(new ValidationResult("Name should be at least 2 characters long.", new[] { nameof(Name) }));
        }
        
        if (string.IsNullOrEmpty(Description) || Description.Trim().Length < 2)
        {
            results.Add(new ValidationResult("Description should be at least 2 characters long.", new[] { nameof(Description) }));
        }
        
        return results;
    }
}