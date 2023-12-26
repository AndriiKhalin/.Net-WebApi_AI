namespace LB3.Interfaces;

public interface IComponentComputerRepository
{
    Task<IEnumerable<ComponentComputer>> GetAll();

    Task<ComponentComputer> GetById(int id);

    Task<ComponentComputer> GetByName(string name);

    Task Add(ComponentComputer componentComputer);

    Task Update(ComponentComputer componentComputer);

    Task Delete(int id);


}