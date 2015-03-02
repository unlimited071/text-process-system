namespace Server.Models
{
    public interface IStatsCalculator
    {
        Stat[] Calculate(string input);
    }
}