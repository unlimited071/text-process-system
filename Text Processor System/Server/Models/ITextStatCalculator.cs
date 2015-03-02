namespace Server.Models
{
    public interface ITextStatCalculator
    {
        Stat Calculate(string input);
    }
}