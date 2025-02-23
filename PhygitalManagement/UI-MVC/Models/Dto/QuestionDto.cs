using PM.BL.Domain.questions;

namespace PM.UI.Web.MVC.Models.Dto;

public class QuestionDto
{
    public int SubthemeId { get; set; }
    public int Position { get; set; }
    public string QuestionText { get; set; }
    public QuestionType QuestionType { get; set; }
    public ICollection<ResponseOptionDto> ResponseOptionsDtos { get; set; }
}
