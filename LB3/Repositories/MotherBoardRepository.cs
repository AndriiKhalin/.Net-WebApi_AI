
using System.Runtime.Intrinsics.Arm;

namespace LB3.Repositories;

public class MotherBoardRepository : IMotherBoardRepository
{
    private ComputerDbContext _context;

    public MotherBoardRepository(ComputerDbContext context)
    {
        _context = context;
    }
    public async Task Add(MotherBoard motherBoard)
    {
        await _context.MotherBoards.AddAsync(motherBoard);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var motherBoard = await _context.MotherBoards.FirstOrDefaultAsync(p => p.Id == id);
        if (motherBoard != null)
        {
            _context.MotherBoards.Remove(motherBoard);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<MotherBoard>> GetAll()
    {
        var motherBoards = await _context.MotherBoards.ToListAsync();
        return motherBoards;
    }

    public async Task<MotherBoard> GetById(int id)
    {
        return await _context.MotherBoards.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<MotherBoard> GetByName(string name)
    {
        throw new NotImplementedException();
    }

    public async Task Update(MotherBoard motherBoard)
    {
        var _motherBoard = await _context.MotherBoards.FirstOrDefaultAsync(x => x.Id == motherBoard.Id);

        if (_motherBoard != null)
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
            _context.Entry(_motherBoard).CurrentValues.SetValues(motherBoard);
            await _context.SaveChangesAsync();
        }
    }
}