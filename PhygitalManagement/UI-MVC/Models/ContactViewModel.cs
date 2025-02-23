using System.ComponentModel.DataAnnotations;

namespace PM.UI.Web.MVC.Models;
public class ContactViewModel
{
    [Required]
    [StringLength(30, MinimumLength = 3)]
    public string Subject { get; set; }
    [Required(ErrorMessage = "Please enter a message.")]
    public string Message { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Phone { get; set; }
}