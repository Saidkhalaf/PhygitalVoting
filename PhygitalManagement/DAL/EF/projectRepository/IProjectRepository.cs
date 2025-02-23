using Microsoft.AspNetCore.Identity;
using PM.BL.Domain.flows;
using PM.BL.Domain.projects;

namespace PM.DAL.EF.projectRepository;

public interface IProjectRepository
{
    IEnumerable<Project> ReadAllProjectsOfUser(IdentityUser user);
    IEnumerable<Project> ReadAllProjectsWithManager();
    void CreateProject(Project project);
    Project ReadProjectWithManager(int id);
    void UpdateProject(Project project);
    Project ReadProjectWithFlows(int id);
    Flow CreateFlow(Flow flow);
    void DeleteProject(Project project);
    Project ReadProjectOfFlow(int flowId);

    public Project GetProjectWithFlowsWithSubthemesWithQuestionsWithUserAnswers(int projectId);
}