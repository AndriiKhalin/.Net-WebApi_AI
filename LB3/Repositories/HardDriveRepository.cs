
namespace LB3.Repositories;

public class HardDriveRepository : IHardDriveRepository
{
    private ComputerDbContext _context;

    public HardDriveRepository(ComputerDbContext context)
    {
        _context = context;
    }
    public async Task Add(HardDrive hardDrive)
    {
        await _context.HardDrives.AddAsync(hardDrive);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var hardDrive = await _context.HardDrives.FirstOrDefaultAsync(p => p.Id == id);
        if (hardDrive != null)
        {
            _context.HardDrives.Remove(hardDrive);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<HardDrive>> GetAll()
    {
        var hardDrives = await _context.HardDrives.ToListAsync();
        return hardDrives;
    }

    public async Task<HardDrive> GetById(int id)
    {
        return await _context.HardDrives.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<HardDrive> GetByName(string name)
    {
        throw new NotImplementedException();
    }

    public async Task Update(HardDrive hardDrive)
    {
        var _hardDrive = await _context.HardDrives.FirstOrDefaultAsync(x => x.Id == hardDrive.Id);

        if (_hardDrive != null)
        {
            _context.Entry(_hardDrive).CurrentValues.SetValues(hardDrive);
            await _context.SaveChangesAsync();
        }
    }
}