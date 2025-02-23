using System.ComponentModel.DataAnnotations;
using PM.BL.Domain.flows;
using PM.BL.Domain.projects;
using PM.DAL.EF.projectRepository;
using PM.DAL.EF.subPlatformAdminRepository;

namespace PM.BL.projectManager;

public class ProjectManager : IProjectManager
{
    private readonly IProjectRepository _repo;
    private readonly ISubPlatformAdminRepository _subPlatformAdminRepo;
    
    public ProjectManager(IProjectRepository repo, ISubPlatformAdminRepository subPlatformAdminRepo)
    {
        _repo = repo;
        _subPlatformAdminRepo = subPlatformAdminRepo;
    }

    public IEnumerable<Project> GetAllProjectsOfUser(string userId)
    {
        var user = _subPlatformAdminRepo.ReadUser(userId);
        
        return _repo.ReadAllProjectsOfUser(user);
    }
    
    public IEnumerable<Project> GetAllProjectsWithManager()
    {
        return _repo.ReadAllProjectsWithManager();
    }

    public void AddProject(string name, string userId)
    {
        var user = _subPlatformAdminRepo.ReadUser(userId);
        
        var project = new Project()
        {
            Name = name,
            ManagerUser = user
        };
        
        Validate(project);

        _repo.CreateProject(project);
    }

    public void UpdateProjectName(int projectId, string projectName)
    {
        var project = _repo.ReadProjectWithManager(projectId);
        project.Name = projectName;

        Validate(project);

        _repo.UpdateProject(project);
    }

    public void UpdateProjectStatus(int projectId, bool projectStatus)
    {
        var project = _repo.ReadProjectWithManager(projectId);
        project.Status = projectStatus;

        Validate(project);

        _repo.UpdateProject(project);
    }
    
    public Project GetProjectWithFlows(int projectId)
    {
        return _repo.ReadProjectWithFlows(projectId);
    }

    public Flow AddFlow(int projectId)
    {
        var project = _repo.ReadProjectWithFlows(projectId);

        var countFlows = project.Flows.Count;
        
        var flow = new Flow();
        flow.Project = project;
        flow.Name = "Enquête " + (countFlows + 1);
        
        Validate(flow);
        
        return _repo.CreateFlow(flow);
    }

    public void RemoveProject(int projectId)
    {
        var project = _repo.ReadProjectWithManager(projectId);
        Validate(project);
        _repo.DeleteProject(project);
    }

    public Project GetProjectOfFlow(int flowId)
    {
        return _repo.ReadProjectOfFlow(flowId);
    }

    public void UpdateParticipantCountOfProject(int flowId)
    {
        var project = GetProjectOfFlow(flowId);
        project.ParticipantCount++;
        
        _repo.UpdateProject(project);
    }

    public Project GetProjectWithFlowsWithSubthemesWithQuestionsWithUserAnswers(int projectId)
    {
        return _repo.GetProjectWithFlowsWithSubthemesWithQuestionsWithUserAnswers(projectId);
    }

    public bool GetStatusOfProject(int flowId)
    {
        var project = _repo.ReadProjectOfFlow(flowId);
        return project.Status;
    }

    private void Validate(Object obj)
    {
        List<ValidationResult> errors = new List<ValidationResult>();

        bool valid = Validator.TryValidateObject(obj, new ValidationContext(obj), errors, validateAllProperties: true);

        if (!valid)
        {
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }

            throw new ValidationException("Please try again...");
        }
    }
    
}

