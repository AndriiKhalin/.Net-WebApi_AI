namespace LB3.Interfaces;

public interface IMotherBoardRepository
{
    Task<IEnumerable<MotherBoard>> GetAll();

    Task<MotherBoard> GetById(int id);

    Task<MotherBoard> GetByName(string name);

    Task Add(MotherBoard motherBoard);

    Task Update(MotherBoard motherBoard);

    Task Delete(int id);

}