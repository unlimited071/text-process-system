﻿using System.Collections.Generic;
using Server.Models;

namespace ServerTests
{
    public class FakeStatsCalculator : IStatsCalculator
    {
        public bool Executed { get; private set; }

        public IEnumerable<Stat> Calculate(string input)
        {
            Executed = true;
            return new Stat[] {};
        }
    }
}