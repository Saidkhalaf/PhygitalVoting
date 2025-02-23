using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PM.BL.Domain.flows;
using PM.BL.Domain.projects;
using PM.BL.Domain.questions;

namespace PM.DAL.EF.projectRepository;

public class ProjectRepository : IProjectRepository
{
    private readonly PhygitalDbContext _context;

    public ProjectRepository(PhygitalDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Project> ReadAllProjectsOfUser(IdentityUser user)
    {
        return _context.Projects.Where(project => project.ManagerUser == user);
    }
    
    public IEnumerable<Project> ReadAllProjectsWithManager()
    {
        return _context.Projects
            .Include(project => project.ManagerUser)
            .AsEnumerable();
    }

    public void CreateProject(Project project)
    {
        _context.Projects.Add(project);
        _context.SaveChanges();
    }

    public Project ReadProjectWithManager(int id)
    {
        return _context.Projects
            .Include(project => project.ManagerUser)
            .SingleOrDefault(project => project.Id == id);
    }

    public void UpdateProject(Project project)
    {
        _context.Projects.Update(project);
        _context.SaveChanges();
    }

    public Project ReadProjectWithFlows(int id)
    {
        return _context.Projects
            .Where(project => project.Id == id)
            .Include(project => project.Flows)
            .FirstOrDefault();
    }

    public Flow CreateFlow(Flow flow)
    {
        _context.Flows.Add(flow);
        _context.SaveChanges();

        return flow;
    }

    public void DeleteProject(Project project)
    {
        _context.Projects.Remove(project);
        _context.SaveChanges();
    }

    public Project ReadProjectOfFlow(int flowId)
    {
        return _context.Projects.SingleOrDefault(project => project.Flows.Any(flow => flow.Id == flowId));
    }

    public Project GetProjectWithFlowsWithSubthemesWithQuestionsWithUserAnswers(int projectId)
    {
        return _context.Projects
            .Include(project => project.Flows)
            .ThenInclude(flow => flow.Subthemes)
            .ThenInclude(subtheme => subtheme.Elements)
            .ThenInclude(element => ((Question)element).Answers)
            .SingleOrDefault(project => project.Id == projectId);
    }
}