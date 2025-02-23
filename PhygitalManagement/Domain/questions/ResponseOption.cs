using System.ComponentModel.DataAnnotations;

namespace PM.BL.Domain.questions;

public class ResponseOption
{
    [Key] public int Id { get; set; }
    public string OptionText { get; set; }
    public Question Question { get; set; }

    public int QuestionId { get; set; }

    public ResponseOption(string optionText)
    {
        OptionText = optionText;
    }

    public ResponseOption()
    {
    }
}