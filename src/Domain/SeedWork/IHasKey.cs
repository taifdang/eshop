namespace Domain.SeedWork;

public interface IHasKey<T>
{
    T Id { get; set; }
}
