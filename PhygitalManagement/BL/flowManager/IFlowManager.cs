using PM.BL.Domain.flows;

namespace PM.BL.flowManager;

public interface IFlowManager
{
    void RemoveFlow(int flowId);
    void UpdateFlow(int flowId, FlowType flowType, string name);
    Flow GetFlowWithSubthemes(int id);
}