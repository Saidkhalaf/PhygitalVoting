using System.ComponentModel.DataAnnotations;

namespace PM.UI.Web.MVC.Models.Dto;

public class ResponseOptionDto
{
    public int Id { get; set; }
    [Required]
    public string OptionText { get; set; }
}