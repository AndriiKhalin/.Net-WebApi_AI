
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;

namespace LB3.Repositories;

public class ComponentComputerRepository : IComponentComputerRepository
{
    private ComputerDbContext _context;

    public ComponentComputerRepository(ComputerDbContext context)
    {
        _context = context;
    }

    public async Task Add(ComponentComputer componentComputer)
    {
        await _context.ComponentComputers.AddAsync(componentComputer);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var componentComputer = await _context.ComponentComputers.FirstOrDefaultAsync(p => p.Id == id);
        if (componentComputer != null)
        {
            _context.ComponentComputers.Remove(componentComputer);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<ComponentComputer>> GetAll()
    {
        var componentComputers = await _context.ComponentComputers.ToListAsync();
        return componentComputers;
    }

    public async Task<ComponentComputer> GetById(int id)
    {
        return await _context.ComponentComputers.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<ComponentComputer> GetByName(string name)
    {
        throw new NotImplementedException();
    }

    public async Task Update(ComponentComputer componentComputer)
    {
        var _componentComputer = await _context.ComponentComputers.FirstOrDefaultAsync(x => x.Id == componentComputer.Id);

        if (_componentComputer != null)
        {
            _context.Entry(_componentComputer).CurrentValues.SetValues(componentComputer);
            await _context.SaveChangesAsync();
        }
    }
}