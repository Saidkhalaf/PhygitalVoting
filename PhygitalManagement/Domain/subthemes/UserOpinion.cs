using System.ComponentModel.DataAnnotations;

namespace PM.BL.Domain.subthemes;

public class UserOpinion
{ 
    [Key]
    public int Id { get; set; } 
    [Required]
    public string Text { get; set; } 
    public int SubthemeId { get; set; }
    
    public UserOpinion()
    {
    }
    
    public UserOpinion(string text, int subthemeId)
    {
        Text = text;
        SubthemeId = subthemeId;
    }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    { 
        var results = new List<ValidationResult>();

        if (string.IsNullOrEmpty(Text) || Text.Trim().Length < 3)
        {
            results.Add(new ValidationResult("Text should be at least 2 characters long.", new[] { nameof(Text) }));
        }
        
        return results;
    }
}