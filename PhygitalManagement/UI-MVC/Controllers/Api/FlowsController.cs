using Microsoft.AspNetCore.Mvc;
using PM.BL.flowManager;
using PM.DAL.EF;
using PM.UI.Web.MVC.Models.Dto;

namespace PM.UI.Web.MVC.Controllers.Api;

[ApiController]
[Route("/api/[controller]")]
public class FlowsController : ControllerBase
{
    private readonly IFlowManager _mgr;
    private readonly UnitOfWork _uow;

    public FlowsController(IFlowManager mgr, UnitOfWork uow)
    {
        _mgr = mgr;
        _uow = uow;
    }
    
    [HttpPut("{flowId}")]
    public IActionResult PutFlow(int flowId, FlowDto flowDto)
    {
        _uow.BeginTransaction();
        _mgr.UpdateFlow(flowId, flowDto.FlowType, flowDto.FlowName);
        _uow.Commit();
        return NoContent();
    }
}