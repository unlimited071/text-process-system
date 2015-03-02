namespace Server.Models
{
    public interface IStatsPersister
    {
        void Persist(string input, params Stat[] stats);
    }
}