using Microsoft.EntityFrameworkCore;
using PM.BL.Domain.elements;
using PM.BL.Domain.questions;

namespace PM.DAL.EF.questionRepository;

public class QuestionRepository : IQuestionRepository
{
    private readonly PhygitalDbContext _context;

    public QuestionRepository(PhygitalDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Question> ReadAllQuestions()
    {
        return _context.Questions
            .Include(question => question.ResponseOptions)
            .AsEnumerable();
    }

    public IEnumerable<Question> ReadAllQuestionsWithSubtheme(int subthemeId)
    {
        return _context.Questions
            .Where(question => question.Subtheme.Id == subthemeId)
            .Include(question => question.ResponseOptions)
            .AsEnumerable();
    }

    public Question ReadQuestionWithSubtheme(int subthemeId, int position)
    {
        var subtheme = _context.Subthemes
            .Include(subtheme1 => subtheme1.Elements)
            .SingleOrDefault(subtheme1 => subtheme1.Id == subthemeId);

        if (subtheme == null || position > subtheme.Elements.Count)
        {
            return null;
        }

        return _context.Questions
            .Where(question => question.Subtheme.Id == subthemeId && question.Position == position)
            .Include(question => question.ResponseOptions)
            .Include(question => question.Subtheme)
            .SingleOrDefault();
    }

    public Question ReadQuestionById(int id)
    {
        return _context.Questions.Find(id);
    }

    public void CreateAnswer(UserAnswer userAnswer)
    {
        _context.UserAnswers.Add(userAnswer);
        _context.SaveChanges();
    }

    public void CreateQuestion(Question question)
    {
        _context.Questions.Add(question);
        _context.SaveChanges();
    }
    
    public void DeleteQuestion(Question question)
    {
        _context.Questions.Remove(question);
        _context.SaveChanges();
    }

    public Element ReadQuestionByElementId(int id)
    {
        return _context.Questions.Include(qs => qs.ResponseOptions)
            .FirstOrDefault(qs => qs.ElementId == id);
    }

    public int ReadTotalQuestionsWithSubtheme(int subthemeId)
    {
        return _context.Subthemes
            .Include(s => s.Questions)
            .Where(s => s.Id == subthemeId)
            .Select(s => s.Questions.Count)
            .FirstOrDefault();
    }
    
    public Question ReadQuestionWithSubthemeById(int id)
    {
        return _context.Questions
            .Include(question => question.Subtheme)
            .FirstOrDefault(question => question.ElementId == id);
    }

    public void RemoveQuestionByElementId(int id)
    {
        var question = _context.Questions.FirstOrDefault(q => q.ElementId == id);
        if (question != null)
        {
            _context.Questions.Remove(question);
            _context.SaveChanges();
        }
    }
    
    public void UpdateQuestion(Question question)
    {
        _context.Questions.Update(question);
        _context.SaveChanges();
    }
}