using Microsoft.EntityFrameworkCore;
using PM.BL.Domain.flows;

namespace PM.DAL.EF.flowRepository;

public class FlowRepository : IFlowRepository
{
    
    private readonly PhygitalDbContext _context;

    public FlowRepository(PhygitalDbContext context)
    {
        _context = context;
    }

    public void DeleteFlow(Flow flow)
    {
        _context.Flows.Remove(flow);
        _context.SaveChanges();
    }

    public Flow ReadFlow(int flowId)
    {
        return _context.Flows.Find(flowId);
    }
    
    public void UpdateFlow(Flow flow)
    {
        _context.Flows.Update(flow);
        _context.SaveChanges();
    }

    public Flow ReadFlowWithSubthemes(int id)
    {
        return _context.Flows
            .Include(flow => flow.Subthemes)
            .SingleOrDefault(flow => flow.Id == id);
    }
}