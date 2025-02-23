using Microsoft.AspNetCore.Mvc;
using PM.BL.flowManager;
using PM.BL.projectManager;
using PM.DAL.EF;

namespace PM.UI.Web.MVC.Controllers;

public class FlowController : Controller
{
    private readonly IFlowManager _flowManager;
    private readonly IProjectManager _projectManager;
    private readonly UnitOfWork _uow;

    public FlowController(IFlowManager flowManager, IProjectManager projectManager, UnitOfWork uow)
    {
        _flowManager = flowManager;
        _projectManager = projectManager;
        _uow = uow;
    }

    [HttpGet]
    public IActionResult Index(int flowId)
    {
        if (!User.Identity.IsAuthenticated)
        {
            _projectManager.UpdateParticipantCountOfProject(flowId);
        }
        
        var flow = _flowManager.GetFlowWithSubthemes(flowId);
        var projectStatus = _projectManager.GetStatusOfProject(flowId);
        if (projectStatus)
        {
            ViewData["projectStatus"] = true;
        }
        return View(flow);
    }
}