namespace Domain.SeedWork;

public interface IEntity<T> : IHasKey<T>, IEntity
{
    
}
public interface IEntity : ITrackable
{
   
}