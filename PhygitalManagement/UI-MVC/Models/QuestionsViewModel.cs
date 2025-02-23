using Microsoft.Build.Framework;
using PM.BL.Domain.elements;
using PM.BL.Domain.questions;

namespace PM.UI.Web.MVC.Models;

public class QuestionsViewModel : Element
{
    public int FlowId { get; set; }
    public string SubthemeName { get; set; }
    public string SubthemeDescription { get; set; }
    [Required]
    public int QuestionPosition { get; set; }
    [Required]
    public string QuestionText { get; set; }
    public QuestionType QuestionType { get; set; }
    [Required]
    public ICollection<ResponseOption> ResponseOptions { get; set; }
    public ICollection<UserAnswer> Answers { get; set; }
    
    public List<Question> Questions { get; set; }

    public QuestionsViewModel()
    {
        Questions = new List<Question>();
    }
}