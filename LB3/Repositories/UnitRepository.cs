

using System.Runtime.Intrinsics.Arm;

namespace LB3.Repositories;

public class UnitRepository : IUnitRepository
{
    private ComputerDbContext _context;

    public UnitRepository(ComputerDbContext context)
    {
        _context = context;
    }
    public async Task Add(Unit unit)
    {
        await _context.Units.AddAsync(unit);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var unit = await _context.Units.FirstOrDefaultAsync(p => p.Id == id);
        if (unit != null)
        {
            _context.Units.Remove(unit);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Unit>> GetAll()
    {
        var units = await _context.Units.ToListAsync();
        return units;
    }

    public async Task<Unit> GetById(int id)
    {
        return await _context.Units.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<Unit> GetByName(string name)
    {
        throw new NotImplementedException();
    }

    public async Task Update(Unit unit)
    {
        var _unit = await _context.Units.FirstOrDefaultAsync(x => x.Id == unit.Id);

        if (_unit != null)
        {
            //_processor.Name = processor.Name;
            //_processor.ProcessorFamily = processor.ProcessorFamily;
            //_processor.ProcessorGeneration = processor.ProcessorGeneration;
            //_processor.SocketType = processor.SocketType;
            //_processor.NumCores = processor.NumCores;
            //_processor.NumThreads = processor.NumThreads;
            //_processor.ClockFrequency = processor.ClockFrequency;
            //_processor.Image = processor.Image;
            //_processor.Price = processor.Price;

            //_context.Update(_processor);
            _context.Entry(_unit).CurrentValues.SetValues(unit);
            await _context.SaveChangesAsync();
        }
    }
}