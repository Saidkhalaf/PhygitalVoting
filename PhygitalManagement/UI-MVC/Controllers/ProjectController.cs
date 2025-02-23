using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PM.BL;
using PM.BL.Domain.projects;
using PM.BL.flowManager;
using PM.BL.projectManager;
using PM.DAL.EF;

namespace PM.UI.Web.MVC.Controllers;

public class ProjectController : Controller
{
    private readonly IProjectManager _projectManager;
    private readonly IFlowManager _flowManager;
    private readonly UnitOfWork _uow;
    
    private readonly UserManager<IdentityUser> _userManager;

    public ProjectController(IProjectManager projectManager, IFlowManager flowManager, UnitOfWork uow, UserManager<IdentityUser> userManager)
    {
        _projectManager = projectManager;
        _flowManager = flowManager;
        _uow = uow;

        _userManager = userManager;
    }
    
    [HttpGet]
    [Authorize]
    public IActionResult Index()
    {
        var userId = _userManager.GetUserId(User);
        
        var projects = _projectManager.GetAllProjectsOfUser(userId);
        return View(projects);
    }
    
    [HttpGet]
    [Authorize]
    public IActionResult IndexAll()
    {
        var projects = _projectManager.GetAllProjectsWithManager();
        
        int totalParticipantCount = 0;
        foreach (var project in projects)
        {
            totalParticipantCount += project.ParticipantCount;
        }

        ViewData["totalParticipantCount"] = totalParticipantCount;
        return View(projects);
    }
    
    [HttpPost]
    public IActionResult Add(string name)
    {
        var userId = _userManager.GetUserId(User);
        
        _uow.BeginTransaction();
        _projectManager.AddProject(name, userId);
        _uow.Commit();
        return RedirectToAction("Index", "Project");
    }
    
    [HttpPost]
    public IActionResult Remove(int projectId)
    {
        _uow.BeginTransaction();
        _projectManager.RemoveProject(projectId);
        _uow.Commit();
        return RedirectToAction("Index", "Project");
    }
    
    [HttpPost]
    public IActionResult PostProject(int projectId)
    {
        return RedirectToAction("Details", "Project", new { projectId });
    }
    
    [HttpGet]
    public IActionResult Details(int projectId)
    {
        var project = _projectManager.GetProjectWithFlows(projectId);
            
        return View(project);
    }
    
    [HttpGet]
    [Authorize]
    public IActionResult Answers(int projectId)
    {
        var project = _projectManager.GetProjectWithFlowsWithSubthemesWithQuestionsWithUserAnswers(projectId);
        return View(project);
    }
    
    [HttpPost]
    public IActionResult AddFlow(int projectId)
    {
        _uow.BeginTransaction();
        _projectManager.AddFlow(projectId);
        _uow.Commit();
        return RedirectToAction("Details", "Project", new { projectId });
    }
    
    [HttpPost]
    public IActionResult RemoveFlow(int flowId, int projectId)
    {
        _uow.BeginTransaction();
        _flowManager.RemoveFlow(flowId);
        _uow.Commit();
        return RedirectToAction("Details", "Project", new { projectId });
    }
}