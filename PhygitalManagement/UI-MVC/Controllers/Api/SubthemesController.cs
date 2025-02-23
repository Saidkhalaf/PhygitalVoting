using Microsoft.AspNetCore.Mvc;
using PM.BL.Domain.questions;
using PM.BL.Domain.subthemes;
using PM.BL.questionManager;
using PM.BL.subthemeManager;
using PM.DAL.EF;
using PM.UI.Web.MVC.Models.Dto;

namespace PM.UI.Web.MVC.Controllers.Api;

[ApiController]
[Route("/api/[controller]")]
public class SubthemesController : ControllerBase
{
    private readonly ISubthemeManager _subthemeManager;
    private readonly IQuestionManager _questionManager;
    private readonly UnitOfWork _uow;

    public SubthemesController(ISubthemeManager subthemeManager, IQuestionManager questionManager, UnitOfWork uow)
    {
        _subthemeManager = subthemeManager;
        _questionManager = questionManager;
        _uow = uow;
    }

    [HttpDelete("{subthemeId}")]
    public IActionResult DeleteSubthemes(int subthemeId)
    {
        _uow.BeginTransaction();
        _subthemeManager.RemoveSubtheme(subthemeId);
        _uow.Commit();
        return Ok();
    }
    
    [HttpPut("{subthemeId}")]
    public ActionResult<SubthemeDto> ModifySubtheme(int subthemeId, SubthemeDto subthemeDto)
    {
        _uow.BeginTransaction();
        Subtheme subtheme = _subthemeManager.GetSubthemeById(subthemeId);
        subtheme.Id = subthemeId;
        subtheme.Name = subthemeDto.Name;
        subtheme.Description = subthemeDto.Description;
        _subthemeManager.UpdateSubtheme(subtheme);

        _uow.Commit();
        return NoContent();
    }

    [HttpPost("{subthemeId}/questions")]
    public IActionResult AddQuestionToSubtheme(int subthemeId, QuestionDto questionDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _uow.BeginTransaction();
        var optionList = new List<ResponseOption>();

        foreach (var respondOptionDto in questionDto.ResponseOptionsDtos)
        {
            optionList.Add(new ResponseOption { OptionText = respondOptionDto.OptionText });
        }

        _questionManager.AddQuestion(subthemeId, questionDto.Position, questionDto.QuestionText, questionDto.QuestionType, optionList);

        _uow.Commit();
        return NoContent();
    }

    
    [HttpDelete("{subthemeId}/questions/{position}")]
    public IActionResult DeleteQuestionsFromSubtheme(int subthemeId, int position)
    {
        _uow.BeginTransaction();
        var question = _questionManager.GetQuestionWithSubtheme(subthemeId, position);
        if (question == null)
        {
            return NotFound();
        }
        _questionManager.RemoveQuestion(question.ElementId);
        _uow.Commit();
        return Ok();
    }
}