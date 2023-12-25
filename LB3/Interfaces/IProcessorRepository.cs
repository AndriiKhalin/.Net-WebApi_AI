using LB3.Features;
using Microsoft.ML;

namespace LB3.Interfaces;

public interface IProcessorRepository
{
    Task<IEnumerable<Processor>> GetAll();

    Task<Processor> GetById(int id);

    Task<Processor> GetByName(string name);

    Task Add(Processor processor);

    Task Update(Processor processor);

    Task Delete(int id);

    float CalculatePerformance(Processor processor);

}