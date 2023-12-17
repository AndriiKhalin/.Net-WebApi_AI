
using LB3.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace LB3.Repositories;

public class ProcessorRepository : IProcessorRepository
{
    private ComputerDbContext _context;

    public ProcessorRepository(ComputerDbContext context)
    {
        _context = context;
    }
    public async Task Add(Processor processor)
    {
        await _context.Processors.AddAsync(processor);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var processor = await _context.Processors.FirstOrDefaultAsync(p => p.Id == id);
        if (processor != null)
        {
            _context.Processors.Remove(processor);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Processor>> GetAll()
    {

        var processors = await _context.Processors.ToListAsync();




        return processors;
    }

    public async Task<Processor> GetById(int id)
    {
        return await _context.Processors.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<Processor> GetByName(string name)
    {
        throw new NotImplementedException();
    }

    public async Task Update(Processor processor)
    {
        var _processor = await _context.Processors.FirstOrDefaultAsync(x => x.Id == processor.Id);

        if (_processor != null)
        {
            _context.Entry(_processor).CurrentValues.SetValues(processor);
            await _context.SaveChangesAsync();
        }
    }
}