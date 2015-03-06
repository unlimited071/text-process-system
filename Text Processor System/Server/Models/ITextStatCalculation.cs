using System;

namespace Server.Models
{
    public interface ITextStatCalculation
    {
        Func<string, Stat> Calculation { get; }
    }
}