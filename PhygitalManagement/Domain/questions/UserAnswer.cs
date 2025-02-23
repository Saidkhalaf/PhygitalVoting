using System.ComponentModel.DataAnnotations;

namespace PM.BL.Domain.questions;

public class UserAnswer
{
    [Key] public string Id { get; set; }
    public Question Question { get; set; }
    public int QuestionId { get; set; }
    public string Answer { get; set; }

    public UserAnswer(string id, int questionId, string answer)
    {
        Id = id;
        QuestionId = questionId;
        Answer = answer;
    }

    public UserAnswer()
    {
    }
}