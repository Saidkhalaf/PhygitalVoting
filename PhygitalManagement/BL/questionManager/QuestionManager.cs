using System.ComponentModel.DataAnnotations;
using PM.BL.Domain.elements;
using PM.BL.Domain.questions;
using PM.DAL.EF.questionRepository;

namespace PM.BL.questionManager;

public class QuestionManager : IQuestionManager
{
    private readonly IQuestionRepository _repo;

    public QuestionManager(IQuestionRepository repo)
    {
        _repo = repo;
    }

    public Question GetQuestionWithSubtheme(int subthemeId, int position)
    {
        return _repo.ReadQuestionWithSubtheme(subthemeId, position);
    }

    public Question GetSubthemeWithQuestionsById(int id)
    {
        return _repo.ReadQuestionWithSubthemeById(id);
    }

    public void DeleteQuestionByElementId(int id)
    {
        _repo.RemoveQuestionByElementId(id);
    }

    public void UpdateQuestions(Question question)
    {
        Validate(question);
        _repo.UpdateQuestion(question);
    }

    public void UpdateQuestion(int subthemeId, int position, string questionText, QuestionType questionType, ICollection<ResponseOption> responseOptions)
    {
        var question = _repo.ReadQuestionWithSubthemeById(subthemeId);

        question.SubthemeId = subthemeId;
        question.Position = position;
        question.QuestionText = questionText;
        question.QuestionType = questionType;
        question.ResponseOptions = responseOptions;
        
        Validate(question);
        _repo.UpdateQuestion(question);
    }
    
    public void AddAnswer(UserAnswer userAnswer)
    {
        if (string.IsNullOrEmpty(userAnswer.Answer))
        {
            throw new ArgumentException("Answer cannot be empty.");
        }
        _repo.CreateAnswer(userAnswer);
    }

    public void AddQuestion(int subthemeId, int position, string questionText, QuestionType questionType, ICollection<ResponseOption> responseOptions)
    {
        var question = new Question
        {
            SubthemeId = subthemeId,
            Position = position,
            QuestionText = questionText,
            QuestionType = questionType,
            ResponseOptions = responseOptions
        };
        Validate(question);
        _repo.CreateQuestion(question);
    }

    public void RemoveQuestion(int questionId)
    {
        var question = _repo.ReadQuestionById(questionId);
        _repo.DeleteQuestion(question);
    }
    
    public Element GetQuestionByElementId(int id)
    {
        return _repo.ReadQuestionByElementId(id);
    }

    public Question GetQuestionById(int questionId)
    {
        return _repo.ReadQuestionById(questionId);
    }
    
    private void Validate(Object obj)
    {
        List<ValidationResult> errors = new List<ValidationResult>();

        bool valid = Validator.TryValidateObject(obj, new ValidationContext(obj), errors, validateAllProperties: true);

        if (!valid)
        {
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }

            throw new ValidationException("Please try again...");
        }
    }
}