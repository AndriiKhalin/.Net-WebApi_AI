namespace LB3.Interfaces;

public interface IHardDriveRepository
{
    Task<IEnumerable<HardDrive>> GetAll();

    Task<HardDrive> GetById(int id);

    Task<HardDrive> GetByName(string name);

    Task Add(HardDrive hardDrive);

    Task Update(HardDrive hardDrive);

    Task Delete(int id);

}