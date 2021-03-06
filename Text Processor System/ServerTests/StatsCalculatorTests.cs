﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Models;

namespace ServerTests
{
    [TestClass]
    public class StatsCalculatorTests
    {
        private const string Input =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam sed elementum elit. Suspendisse elementum erat a nisi dapibus, sed feugiat massa ultricies. Nunc pulvinar auctor elementum. Aliquam feugiat tellus ac eros blandit, at sodales dui mattis. Duis quis lectus quam. Etiam consectetur, turpis id finibus sagittis, risus sem rhoncus diam, id venenatis ex enim non ante. Mauris tortor leo, tincidunt sed tempor quis, accumsan vitae lorem. Sed ut euismod sapien. Donec feugiat libero dolor, vitae placerat neque dignissim vel. Sed a gravida dolor, eget ultrices tellus. Duis rutrum, turpis at fermentum tempus, enim ex porta felis, et imperdiet dui justo in urna. Duis eget mollis ante. Donec sed dui gravida dolor sodales dignissim. Maecenas pharetra vestibulum tempor. Suspendisse sit amet velit egestas, scelerisque urna vitae, maximus purus. Sed quis consectetur diam.\r\nNunc eget tincidunt risus. Nam vestibulum efficitur nisl, id auctor augue congue eget. Lorem ipsum dolor sit amet, consectetur adipiscing elit. In non risus sit amet sapien bibendum ultricies vitae elementum nunc. Vestibulum ac scelerisque orci, sed tincidunt ante. Suspendisse imperdiet suscipit lorem eu egestas. Aenean blandit, ligula ac vestibulum vestibulum, orci quam vehicula diam, id iaculis orci mi et ex. Suspendisse tempor lectus facilisis dui commodo fringilla.\r\nIn id fringilla felis, id ultricies erat. In eget tincidunt libero, vel ultrices nibh. Fusce ex orci, iaculis et tristique pretium, gravida at mauris. Phasellus et convallis est. Cras et lacus id enim accumsan sollicitudin. Duis aliquet ex quam, quis tristique tellus euismod in. Sed vitae velit sed ex elementum dapibus. Pellentesque vel nunc tellus.\r\nSed cursus nisl et magna venenatis lobortis. Sed nisl felis, semper quis lorem et, luctus consequat nulla. Mauris ac ligula nibh. In volutpat viverra erat. Sed quis lacus pellentesque, sollicitudin nibh ut, laoreet lorem. Sed quis neque ut orci luctus tincidunt ac vel odio. Quisque tempus vitae purus vel posuere. Aliquam placerat quam quis ligula tristique aliquet ut eu justo. Ut consectetur ut lectus eget interdum. Proin vestibulum quis nisi quis efficitur. Aenean non fermentum dolor. Phasellus sed pretium odio. Pellentesque sem nisi, dictum ac hendrerit vestibulum, euismod at quam.\r\nAliquam erat volutpat. Nunc finibus tincidunt tortor, vel porttitor nisl fermentum at. Curabitur sit amet mollis leo, et efficitur ipsum. Donec pellentesque lobortis tellus, accumsan finibus nisl aliquet vel. In pellentesque felis quam, vel laoreet purus pulvinar nec. Sed posuere, erat et luctus consectetur, tellus velit finibus ex, nec faucibus purus nisl non augue. Sed eget nisi facilisis, commodo diam sit amet, dictum urna. Nulla finibus vel arcu nec scelerisque. Cras egestas, diam vitae congue blandit, metus ligula pulvinar nisi, laoreet blandit nulla dui eget risus. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Ut ac nisi lectus. Morbi varius est nisi, eget tristique lectus varius at. Sed fermentum turpis vitae neque volutpat tempus.\r\n";

        [TestMethod]
        public void MustExcecuteAllRegisteredCalculations()
        {
            var calulation1 = new FakeCalculation();
            var calulation2 = new FakeCalculation();
            ITextStatCalculation[] calculations = {calulation1, calulation2};
            IStatsCalculator statsCalculator = new StatsCalculator(calculations);

            IEnumerable<Stat> stats = statsCalculator.Calculate(Input);

            Assert.AreEqual(calculations.Length, stats.Count());
            Assert.IsTrue(calulation1.Executed && calulation2.Executed);
        }

        [TestMethod]
        public void MustCallCalculator()
        {
            var calculator = new FakeStatsCalculator();
            IStatsPersister persister = new FakePersister();
            var processor = new TextStatsProcessor(calculator, persister);

            Task<bool> addTextAsync = processor.AddTextAsync(Input);
            processor.Complete();
            addTextAsync.Wait();

            Assert.IsTrue(calculator.Executed);
        }

        [TestMethod]
        public void MustCallPersister()
        {
            IStatsCalculator calculator = new FakeStatsCalculator();
            var persister = new FakePersister();
            var processor = new TextStatsProcessor(calculator, persister);

            Task<bool> addTextAsync = processor.AddTextAsync(Input);
            processor.Complete();
            addTextAsync.Wait();

            Assert.IsTrue(persister.Executed);
        }
    }

    public class FakeCalculation : ITextStatCalculation
    {
        public bool Executed { get; private set; }

        public Func<string, Stat> Calculation
        {
            get { return Calculate; }
        }

        private Stat Calculate(string arg)
        {
            Executed = true;
            return new Stat();
        }
    }
}