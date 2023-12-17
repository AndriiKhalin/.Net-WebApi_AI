
using System.Diagnostics;

namespace LB3.Repositories;

public class VideoCardRepository : IVideoCardRepository
{
    private ComputerDbContext _context;

    public VideoCardRepository(ComputerDbContext context)
    {
        _context = context;
    }
    public async Task Add(VideoCard videoCard)
    {
        await _context.VideoCards.AddAsync(videoCard);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var videoCard = await _context.VideoCards.FirstOrDefaultAsync(p => p.Id == id);
        if (videoCard != null)
        {
            _context.VideoCards.Remove(videoCard);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<VideoCard>> GetAll()
    {
        var videoCards = await _context.VideoCards.ToListAsync();
        return videoCards;
    }

    public async Task<VideoCard> GetById(int id)
    {
        return await _context.VideoCards.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<VideoCard> GetByName(string name)
    {
        throw new NotImplementedException();
    }

    public async Task Update(VideoCard videoCard)
    {
        var _videoCard = await _context.VideoCards.FirstOrDefaultAsync(x => x.Id == videoCard.Id);

        if (_videoCard != null)
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
            _context.Entry(_videoCard).CurrentValues.SetValues(videoCard);
            await _context.SaveChangesAsync();
        }
    }
}