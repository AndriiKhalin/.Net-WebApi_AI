namespace LB3.Interfaces;

public interface IRamRepository
{
    Task<IEnumerable<Ram>> GetAll();

    Task<Ram> GetById(int id);

    Task<Ram> GetByName(string name);

    Task Add(Ram ram);

    Task Update(Ram ram);

    Task Delete(int id);

}