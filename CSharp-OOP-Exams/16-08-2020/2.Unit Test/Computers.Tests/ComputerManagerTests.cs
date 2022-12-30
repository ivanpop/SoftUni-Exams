using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Computers.Tests
{
    public class Tests
    {
        Computer first;
        Computer second;
        ComputerManager manager;

        [SetUp]
        public void Setup()
        {
            first = new Computer("IBM", "ZenBook", 1900);
            second = new Computer("HP", "ProBook", 1300);
            manager = new ComputerManager();
        }

        [Test]
        public void Ctor()
        {
            List<Computer> list = new List<Computer>();
            CollectionAssert.AreEqual(list, manager.Computers);
        }

        [Test]
        public void AddItems()
        {
            manager.AddComputer(first);
            Assert.AreEqual(1, manager.Count);

            manager.AddComputer(second);
            Assert.AreEqual(2, manager.Count);

            List<Computer> list = new List<Computer>() { first, second };
            Assert.That(list, Is.EquivalentTo(manager.Computers));
        }

        [Test]
        public void RemoveItems()
        {
            manager.AddComputer(first);
            manager.AddComputer(second);

            List<Computer> list = new List<Computer>() { second };
            
            var computer = manager.RemoveComputer("IBM", "ZenBook");
            Assert.AreSame(computer, first);

            Assert.AreEqual(1, manager.Count);
            Assert.That(list, Is.EquivalentTo(manager.Computers));
        }

        [Test]
        public void RemoveIncorrectItems()
        {
            Assert.Throws<ArgumentException>(() => manager.RemoveComputer("IBM", "blabla"));
            Assert.Throws<ArgumentException>(() => manager.RemoveComputer("blabla", "IBM"));
        }

        [Test]
        public void AddDuplicateItem()
        {
            manager.AddComputer(first);          

            Assert.Throws<ArgumentException>(() =>
            {
                manager.AddComputer(first);
            },
            "This computer already exists.");
        }

        [Test]
        public void AddNullItem()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                manager.AddComputer(null);
            });
        }

        [Test]
        public void GetNullItem()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                manager.GetComputer(null, "Probook");
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                manager.GetComputer("IBM", null);
            });

            Assert.Throws<ArgumentException>(() =>
            {
                manager.GetComputer("dgsd", "sfdgsdfg");
            });
        }

        [Test]
        public void GetComputersByManufacturer()
        {
            manager.AddComputer(first);

            Assert.Throws<ArgumentNullException>(() =>
            {
                manager.GetComputersByManufacturer(null);
            });

            List<Computer> list = new List<Computer>() { first };
            Assert.That(list, Is.EquivalentTo(manager.GetComputersByManufacturer("IBM")));

            list = manager.GetComputersByManufacturer("blabla").ToList();
            Assert.IsEmpty(list);
        }
    }
}