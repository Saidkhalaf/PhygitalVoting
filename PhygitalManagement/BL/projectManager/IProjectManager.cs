using PM.BL.Domain.flows;
using PM.BL.Domain.projects;

namespace PM.BL.projectManager;

public interface IProjectManager
{
    IEnumerable<Project> GetAllProjectsOfUser(string userId);
    IEnumerable<Project> GetAllProjectsWithManager();
    void AddProject(string name, string userId);
    void UpdateProjectName(int projectId, string projectName);
    void UpdateProjectStatus(int projectId, bool projectName);
    Project GetProjectWithFlows(int projectId);
    Flow AddFlow(int projectId);
    void RemoveProject(int projectId);
    Project GetProjectOfFlow(int flowId);
    void UpdateParticipantCountOfProject(int flowId);
    Project GetProjectWithFlowsWithSubthemesWithQuestionsWithUserAnswers(int projectId);
    bool GetStatusOfProject(int flowId);
}