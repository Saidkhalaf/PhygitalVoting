using PM.BL.Domain;
using PM.BL.Domain.elements;
using PM.BL.Domain.questions;

namespace PM.DAL.EF.questionRepository;

public interface IQuestionRepository
{
    IEnumerable<Question> ReadAllQuestions();
    IEnumerable<Question> ReadAllQuestionsWithSubtheme(int subthemeId);
    Question ReadQuestionWithSubtheme(int subthemeId, int position);
    void UpdateQuestion(Question question);
    void CreateAnswer(UserAnswer userAnswer);
    void CreateQuestion(Question question);
    public void DeleteQuestion(Question question);
    public Element ReadQuestionByElementId(int id);
    int ReadTotalQuestionsWithSubtheme(int subthemeId);
    public Question ReadQuestionWithSubthemeById(int id);
    void RemoveQuestionByElementId(int id);
    public Question ReadQuestionById(int id);
}