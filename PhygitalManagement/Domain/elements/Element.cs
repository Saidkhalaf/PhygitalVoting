using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using PM.BL.Domain.questions;
using PM.BL.Domain.subthemes;

namespace PM.BL.Domain.elements;

public class Element
{
    [Key] public int ElementId { get; set; }
    public string Name { get; set; }
    [Required] public int Position { get; set; }
    public string Image { get; set; }
    public string Video { get; set; }
    public Subtheme Subtheme { get; set; }
    public Question Question { get; set; }
    public int SubthemeId { get; set; }

    public Element()
    {
    }

    public Element(int position)
    {
        Position = position;
    }

    public Element(int elementId, string name, int position, string image, string video, Subtheme subtheme,
        Question question)
    {
        ElementId = elementId;
        Name = name;
        Position = position;
        Image = image;
        Video = video;
        Subtheme = subtheme;
        Question = question;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        if (string.IsNullOrEmpty(Name) || Name.Trim().Length < 2)
        {
            results.Add(new ValidationResult("Name should be at least 2 characters long.", new[] { nameof(Name) }));
        }

        if (Position < 0)
        {
            results.Add(new ValidationResult("Position should be a non-negative value.", new[] { nameof(Position) }));
        }

        if (string.IsNullOrEmpty(Image) || !IsValidUrl(Image))
        {
            results.Add(new ValidationResult("Invalid image URL.", new[] { nameof(Image) }));
        }

        if (string.IsNullOrEmpty(Video) || !IsValidUrl(Video))
        {
            results.Add(new ValidationResult("Invalid video URL.", new[] { nameof(Video) }));
        }

        return results;
    }

    public bool IsValidUrl(string url)
    {
        var pattern = @"^(http|https)://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(/\S*)?$";
        var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        return regex.IsMatch(url);
    }
}