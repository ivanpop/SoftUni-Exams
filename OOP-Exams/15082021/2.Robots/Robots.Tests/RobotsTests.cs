namespace Robots.Tests
{
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Numerics;

    [TestFixture]
    public class RobotsTests
    {
        Robot first;
        Robot second;
        RobotManager manager;

        [SetUp]
        public void Setup()
        {
            first = new Robot("Robocop", 1000);
            second = new Robot("T1000", 1000);
        }

        [Test]
        public void Ctor()
        {
            manager = new RobotManager(5);
            Assert.AreEqual(5, manager.Capacity);
            Assert.AreEqual(0, manager.Count);
        }

        [TestCase(-1)]
        [TestCase(-50)]
        public void InvalidCapacity(int value)
        {
            Assert.Throws<ArgumentException>(
                () => manager = new RobotManager(value));
        }

        [Test]
        public void Add()
        {
            manager = new RobotManager(2);

            manager.Add(first);
            Assert.AreEqual(1, manager.Count);

            Assert.Throws<InvalidOperationException>(() => manager.Add(first));
            Assert.AreEqual(1, manager.Count);

            manager.Add(second);
            Assert.AreEqual(2, manager.Count);

            Assert.Throws<InvalidOperationException>(() => manager.Add(new Robot("T101", 100)));
            Assert.AreEqual(2, manager.Count);
        }

        [Test]
        public void Remove()
        {
            manager = new RobotManager(2);

            manager.Add(first);
            manager.Add(second);

            manager.Remove("Robocop");
            Assert.AreEqual(1, manager.Count);

            Assert.Throws<InvalidOperationException>(() => manager.Remove("Robocop"));
            Assert.AreEqual(1, manager.Count);

            manager.Remove("T1000");
            Assert.AreEqual(0, manager.Count);
        }

        [Test]
        public void Work()
        {
            manager = new RobotManager(2);
            manager.Add(first);
            manager.Add(second);

            manager.Work("Robocop", "something", 100);
            Assert.AreEqual(900, first.Battery);

            manager.Work("Robocop", "something", -100);
            Assert.AreEqual(1000, first.Battery);

            Assert.Throws<InvalidOperationException>(() => manager.Work("BlaBla", "something", 100));
            Assert.AreEqual(1000, first.Battery);

            Assert.Throws<InvalidOperationException>(() => manager.Work("Robocop", "something", 1100));
            Assert.AreEqual(1000, first.Battery);
        }

        [Test]
        public void Charge()
        {
            manager = new RobotManager(2);
            manager.Add(first);
            manager.Add(second);
            
            manager.Work("Robocop", "something", 100);

            manager.Charge("Robocop");
            Assert.AreEqual(1000, first.Battery);

            Assert.Throws<InvalidOperationException>(() => manager.Charge("BlaBla"));
        }
    }
}