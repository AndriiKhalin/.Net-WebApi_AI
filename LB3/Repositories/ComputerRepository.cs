
using System.Runtime.Intrinsics.Arm;

namespace LB3.Repositories;

public class ComputerRepository : IComputerRepository
{
    private ComputerDbContext _context;

    public ComputerRepository(ComputerDbContext context)
    {
        _context = context;
    }


    public async Task Add(Computer computer)
    {
        await _context.Computers.AddAsync(computer);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var computer = await _context.Computers.FirstOrDefaultAsync(p => p.Id == id);
        if (computer != null)
        {
            _context.Computers.Remove(computer);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Computer>> GetAll()
    {
        var computers = await _context.Computers.ToListAsync();
        return computers;
    }

    public async Task<Computer> GetById(int id)
    {
        return await _context.Computers.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<Computer> GetByName(string name)
    {
        throw new NotImplementedException();
    }

    public async Task Update(Computer computer)
    {
        var _computer = await _context.Computers.FirstOrDefaultAsync(x => x.Id == computer.Id);

        if (_computer != null)
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
            _context.Entry(_computer).CurrentValues.SetValues(computer);
            await _context.SaveChangesAsync();
        }
    }
}