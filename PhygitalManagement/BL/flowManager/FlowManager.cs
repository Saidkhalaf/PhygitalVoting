using System.ComponentModel.DataAnnotations;
using PM.BL.Domain.flows;
using PM.DAL.EF.flowRepository;

namespace PM.BL.flowManager;

public class FlowManager : IFlowManager
{
    private readonly IFlowRepository _repo;

    public FlowManager(IFlowRepository repo)
    {
        _repo = repo;
    }

    public void RemoveFlow(int flowId)
    {
        var flow = _repo.ReadFlow(flowId);
        _repo.DeleteFlow(flow);
    }

    public void UpdateFlow(int flowId, FlowType flowType, string name)
    {
        var flow = _repo.ReadFlow(flowId);
        flow.FlowType = flowType;
        flow.Name = name;
        
        Validate(flow);
        
        _repo.UpdateFlow(flow);
    }
    
    public Flow GetFlowWithSubthemes(int id)
    {
        return _repo.ReadFlowWithSubthemes(id);
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