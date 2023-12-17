
namespace LB3.Interfaces;

public interface IComputerRepository
{
    Task<IEnumerable<Computer>> GetAll();

    Task<Computer> GetById(int id);

    Task<Computer> GetByName(string name);

    Task Add(Computer computer);

    Task Update(Computer computer);

    Task Delete(int id);


}