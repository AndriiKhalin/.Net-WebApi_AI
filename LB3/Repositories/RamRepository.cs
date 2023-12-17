
namespace LB3.Repositories;

public class RamRepository : IRamRepository
{
    private ComputerDbContext _context;

    public RamRepository(ComputerDbContext context)
    {
        _context = context;
    }
    public async Task Add(Ram ram)
    {
        await _context.Rams.AddAsync(ram);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var ram = await _context.Rams.FirstOrDefaultAsync(p => p.Id == id);
        if (ram != null)
        {
            _context.Rams.Remove(ram);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Ram>> GetAll()
    {
        var rams = await _context.Rams.ToListAsync();
        return rams;
    }

    public async Task<Ram> GetById(int id)
    {
        return await _context.Rams.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<Ram> GetByName(string name)
    {
        throw new NotImplementedException();
    }

    public async Task Update(Ram ram)
    {
        var _ram = await _context.Rams.FirstOrDefaultAsync(x => x.Id == ram.Id);

        if (_ram != null)
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
            _context.Entry(_ram).CurrentValues.SetValues(ram);
            await _context.SaveChangesAsync();
        }
    }
}