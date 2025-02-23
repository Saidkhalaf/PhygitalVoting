using Microsoft.AspNetCore.Mvc;
using PM.BL.Domain.elements;
using PM.BL.Domain.questions;
using PM.BL.questionManager;
using PM.DAL.EF;
using PM.UI.Web.MVC.Models.Dto;

namespace PM.UI.Web.MVC.Controllers.Api;

[ApiController]
[Route("/api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionManager _mgr;
    private readonly UnitOfWork _uow;

    public QuestionsController(IQuestionManager mgr, UnitOfWork uow)
    {
        _mgr = mgr;
        _uow = uow;
    }

    [HttpGet("{subthemeId}/{position}")]
    public IActionResult GetNextQuestion(int subthemeId, int position)
    {
        _uow.BeginTransaction();
        var question = _mgr.GetQuestionWithSubtheme(subthemeId, ++position);
        if (question == null)
        {
            return NotFound(new { message = "No more questions in this subtheme", subthemeId });
        }

        QuestionDto questionDto = new QuestionDto
        {
            Position = question.Position,
            QuestionText = question.QuestionText,
            QuestionType = question.QuestionType,
            SubthemeId = question.Subtheme.Id
        };

        questionDto.ResponseOptionsDtos = new List<ResponseOptionDto>();

        foreach (var option in question.ResponseOptions)
        {
            questionDto.ResponseOptionsDtos.Add(new ResponseOptionDto()
            {
                OptionText = option.OptionText
            });
        }

        _uow.Commit();
        return Ok(questionDto);
    }

    [HttpPost("answers")]
    public IActionResult SubmitAnswer(SubmitAnswerDto dto)
    {
        _uow.BeginTransaction();
        var userAnswer = new UserAnswer(dto.Id, dto.QuestionId, dto.Answer);
        _mgr.AddAnswer(userAnswer);

        _uow.Commit();
        return Ok();
    }

    [HttpGet("{subthemeId}/previous/{position}")]
    public QuestionDto GetPreviousQuestion(int subthemeId, int position)
    {
        var question = _mgr.GetQuestionWithSubtheme(subthemeId, position - 1);
        QuestionDto questionDto = new QuestionDto
        {
            Position = question.Position,
            QuestionText = question.QuestionText,
            QuestionType = question.QuestionType,
            SubthemeId = question.Subtheme.Id
        };

        questionDto.ResponseOptionsDtos = new List<ResponseOptionDto>();

        foreach (var option in question.ResponseOptions)
        {
            questionDto.ResponseOptionsDtos.Add(new ResponseOptionDto()
            {
                OptionText = option.OptionText
            });
        }

        return questionDto;
    }

    [HttpPut("{questionId}")]
    public ActionResult<QuestionDto> Update(int questionId, NewQuestionDto questionDto)
    {
        _uow.BeginTransaction();
        Element questionByElementId = _mgr.GetQuestionByElementId(questionId);
        Question question = (Question)questionByElementId;
        question.QuestionText = questionDto.QuestionText;
        question.QuestionType = questionDto.QuestionType;
        question.ElementId = questionId;
        question.ResponseOptions = new List<ResponseOption>();
        foreach (ResponseOptionDto questionDtoResponseOption in questionDto.ResponseOptions)
        {
            question.ResponseOptions.Add(new ResponseOption(questionDtoResponseOption.OptionText));
        }

        _mgr.UpdateQuestions(question);
        _uow.Commit();
        return NoContent();
    }
}