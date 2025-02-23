using Microsoft.AspNetCore.Mvc;
using PM.BL.Domain.questions;
using PM.BL.Domain.subthemes;
using PM.BL.questionManager;
using PM.BL.subthemeManager;
using PM.DAL.EF;


namespace PM.UI.Web.MVC.Controllers;

public class QuestionController : Controller
{
    private readonly IQuestionManager _questionManager;
    private readonly ISubthemeManager _subthemeManager;
    private readonly PhygitalDbContext _context;
    private readonly UnitOfWork _uow;

    public QuestionController(IQuestionManager questionManager, ISubthemeManager subthemeManager, PhygitalDbContext context, UnitOfWork uow)
    {
        _questionManager = questionManager;
        _subthemeManager = subthemeManager;
        _context = context;
        _uow = uow;
    }

    [HttpGet]
    public IActionResult Index(int subthemeId)
    {
        var question = _questionManager.GetQuestionWithSubtheme(subthemeId, 1);
        if (question == null)
        {
            var nextSubthemeId = subthemeId + 1;
            var aantalSubthemes = _subthemeManager.GetTotalSubthemes();
            if (nextSubthemeId <= aantalSubthemes)
            {
                ViewBag.NextSubthemeId = nextSubthemeId;
                return View("SubthemeCompleted");
            }
        }

        var totalSubthemes = _subthemeManager.GetTotalSubthemes();
        ViewData["TotalSubthemes"] = totalSubthemes;
        if (subthemeId <= 1)
        {
            TempData["Message"] = "You are currently viewing the first subtheme.";
        }
        else if (subthemeId >= totalSubthemes)
        {
            TempData["Message"] = "You have reached the last subtheme.";
        }

        var subtheme = _subthemeManager.GetSubthemeById(subthemeId);
        ViewData["Subtheme"] = subtheme;

        return View(question);
    }

    [HttpGet]
    public IActionResult SubthemeCompleted(int nextSubthemeId)
    {
        return View();
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var questionElement = _questionManager.GetQuestionByElementId(id);
        Question question = (Question)questionElement;
        ViewData["subthemeId"] = questionElement.SubthemeId;
        return View(question);
    }


    [HttpPost]
    public async Task<IActionResult> SubmitOpinion(string opinion, int subthemeId)
    {
        _uow.BeginTransaction();
        var newOpinion = new UserOpinion
        {
            Text = opinion,
            SubthemeId = subthemeId
        };

        _context.UserOpinions.Add(newOpinion);
        _context.SaveChangesAsync();

        _uow.Commit();
        return Json(new { succes = true, message = "Your opinion has been submitted.", opinion = newOpinion });
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var question = id;
        if (question != null)
        {
            _questionManager.DeleteQuestionByElementId(id);
            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }
            return RedirectToAction("Index");
        }

        return NotFound(new { message = "No more questions in this subtheme" });
    }
}