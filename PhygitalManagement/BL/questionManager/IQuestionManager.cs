using PM.BL.Domain.elements;
using PM.BL.Domain.questions;

namespace PM.BL.questionManager;

public interface IQuestionManager
{
    Question GetQuestionWithSubtheme(int subthemeId, int position);
    void AddAnswer(UserAnswer userAnswer);
    void AddQuestion(int subthemeId, int position, string questionText, QuestionType questionType,
        ICollection<ResponseOption> responseOptions);
    Element GetQuestionByElementId(int id);
    Question GetSubthemeWithQuestionsById(int id);
    void DeleteQuestionByElementId(int id);
    void UpdateQuestions(Question question);
    void RemoveQuestion(int questionId);
    Question GetQuestionById(int questionId);
    void UpdateQuestion(int subthemeId, int position, string questionText, QuestionType questionType,
        ICollection<ResponseOption> responseOptions);
}