namespace ThirtyMinutes.Persistence
{
    public interface IGameRepository
    {
        Game GetById(int id);
    }
}