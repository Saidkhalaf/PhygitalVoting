using PM.BL.Domain.questions;

namespace PM.UI.Web.MVC.Models.Dto;

public class NewQuestionDto
{
    public string QuestionText { get; set; }
    public QuestionType QuestionType { get; set; }
    public List<ResponseOptionDto> ResponseOptions { get; set; }
}
