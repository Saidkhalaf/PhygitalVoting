using System.ComponentModel.DataAnnotations;
using PM.BL.Domain.elements;
using PM.BL.Domain.subthemes;
using System.Collections.Generic;

namespace PM.BL.Domain.questions;

public class Question : Element
{
    [Required] public string QuestionText { get; set; }
    public QuestionType QuestionType { get; set; }
    [Required] public ICollection<ResponseOption> ResponseOptions { get; set; }
    public ICollection<UserAnswer> Answers { get; set; }

    public Question()
    {
    }

    public Question(int position, string questionText, QuestionType questionType, List<ResponseOption> responseOptions,
        Subtheme subtheme)
        : base(position)
    {
        QuestionText = questionText;
        QuestionType = questionType;
        ResponseOptions = responseOptions;
        Subtheme = subtheme;
    }
}