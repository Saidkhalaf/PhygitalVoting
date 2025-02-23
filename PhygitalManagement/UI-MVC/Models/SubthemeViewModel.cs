using System.ComponentModel.DataAnnotations;

namespace PM.UI.Web.MVC.Models;

public class SubthemeViewModel
{
    public int Id { get; set; }
    public int FlowId { get; set; }
    [Required] public string Name { get; set; }
    [StringLength(150, MinimumLength = 2, ErrorMessage = "At least 2 character, maximum 150 characters")]
    [Required]
    public string Description { get; set; }
    public string Image { get; set; }
    public bool IsActivated { get; set; }
}