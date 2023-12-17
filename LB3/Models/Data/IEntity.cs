namespace LB3.Models.Data;

public interface IEntity
{
    int Id { get; }
    string Name { get; }
    float Price { get; }
}