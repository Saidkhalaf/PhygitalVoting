using PM.BL.Domain.flows;
using PM.BL.Domain.projects;

namespace PM.DAL.EF.flowRepository;

public interface IFlowRepository
{ 
    Flow ReadFlow(int flowId);
    void UpdateFlow(Flow flow);
    void DeleteFlow(Flow flow);
    Flow ReadFlowWithSubthemes(int id);
}