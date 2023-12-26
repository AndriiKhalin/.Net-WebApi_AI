
using Microsoft.ML.Data;
using System.Runtime.Intrinsics.Arm;

namespace LB3.Repositories;

public class ComputerRepository : IComputerRepository
{
    private ComputerDbContext _context;
    private readonly MLContext _mlContext;
    private readonly ITransformer _mlModel;
    private PredictionEngine<ModelInput, ModelOutput> _predictionEngine;

    public class ModelInput
    {
        [LoadColumn(0)]
        [ColumnName(@"Производитель")]
        public string? Производитель { get; set; }

        [LoadColumn(1)]
        [ColumnName(@"Серия")]
        public string? Серия { get; set; }

        [LoadColumn(2)]
        [ColumnName(@"Поколение")]
        public float Поколение { get; set; }

        [LoadColumn(3)]
        [ColumnName(@"Модель")]
        public string? Модель { get; set; }

        [LoadColumn(4)]
        [ColumnName(@"Количество ядер")]
        public float Количество_ядер { get; set; }

        [LoadColumn(5)]
        [ColumnName(@"Количество потоков")]
        public float Количество_потоков { get; set; }

        [LoadColumn(6)]
        [ColumnName(@"Тактовая частота ядер")]
        public float Тактовая_частота_ядер { get; set; }

        [LoadColumn(7)]
        [ColumnName(@"Производительность")]
        public float Производительность { get; set; }

    }
    public class ModelOutput
    {
        [ColumnName(@"Производитель")]
        public float[] Производитель { get; set; }

        [ColumnName(@"Серия")]
        public float[] Серия { get; set; }

        [ColumnName(@"Поколение")]
        public float Поколение { get; set; }

        [ColumnName(@"Модель")]
        public float[] Модель { get; set; }

        [ColumnName(@"Количество ядер")]
        public float Количество_ядер { get; set; }

        [ColumnName(@"Количество потоков")]
        public float Количество_потоков { get; set; }

        [ColumnName(@"Тактовая частота ядер")]
        public float Тактовая_частота_ядер { get; set; }

        [ColumnName(@"Производительность")]
        public float Производительность { get; set; }

        [ColumnName(@"Features")]
        public float[] Features { get; set; }

        [ColumnName(@"Score")]
        public float Score { get; set; }

    }
    public ComputerRepository(ComputerDbContext context, MLContext mlContext)
    {
        _context = context;
        _mlContext = mlContext;

        var mlModelPath = Path.GetFullPath("D:\\IT\\.NET_LB\\AI\\ProcessorModel\\ProcessorModel\\ProcessorModel.mlnet");
        _mlModel = _mlContext.Model.Load(mlModelPath, out _);
        _predictionEngine = _mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(_mlModel);
    }

    public float CalculatePerformance(Processor processor)
    {


        var input = new ModelInput
        {
            Производитель = processor.ProcessorManufacturer,
            Серия = processor.ProcessorSeries,
            Поколение = processor.ProcessorGeneration,
            Модель = processor.ProcessorModel,
            Количество_ядер = processor.NumCores,
            Количество_потоков = processor.NumThreads,
            Тактовая_частота_ядер = processor.ClockFrequency
        };

        var predict = _predictionEngine.Predict(input);

        return predict.Score;
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

    public async Task<Computer?> GetById(int id)
    {
        return await _context.Computers.Include(x => x.ComponentComputer).Include(x => x.ComponentComputer!.Processor).FirstOrDefaultAsync(x => x.Id == id);
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