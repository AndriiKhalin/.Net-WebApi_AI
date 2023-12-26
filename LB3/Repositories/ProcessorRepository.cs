
using LB3.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace LB3.Repositories;

public class ProcessorRepository : IProcessorRepository
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
    public ProcessorRepository(ComputerDbContext context, MLContext mlContext)
    {
        _context = context;
        _mlContext = mlContext;

        var mlModelPath = Path.GetFullPath("D:\\IT\\.NET_LB\\AI\\ProcessorModel\\ProcessorModel\\ProcessorModel.mlnet");
        _mlModel = _mlContext.Model.Load(mlModelPath, out _);
        _predictionEngine = _mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(_mlModel);
    }

    public async Task Add(Processor processor)
    {
        await _context.Processors.AddAsync(processor);
        await _context.SaveChangesAsync();
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