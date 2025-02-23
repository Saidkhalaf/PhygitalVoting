using Microsoft.AspNetCore.Mvc;
using PM.BL.projectManager;
using PM.DAL.EF;
using PM.UI.Web.MVC.Models.Dto;

namespace PM.UI.Web.MVC.Controllers.Api;

[ApiController]
[Route("/api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectManager _projectManager;
    private readonly UnitOfWork _uow;

    public ProjectsController(IProjectManager projectManager, UnitOfWork uow)
    {
        _projectManager = projectManager;
        _uow = uow;
    }
    
    [HttpPut("{projectId}")]
    public IActionResult PutProject(int projectId, ProjectDto projectDto)
    {
        _uow.BeginTransaction();
        _projectManager.UpdateProjectName(projectId, projectDto.projectName);
        _uow.Commit();
        return NoContent();
    }
    
    [HttpPut("{projectId}/status")]
    public IActionResult PutProjectStatus(int projectId, ProjectDto projectDto)
    {
        _uow.BeginTransaction();
        _projectManager.UpdateProjectStatus(projectId, projectDto.projectStatus);
        _uow.Commit();
        return NoContent();
    }
}