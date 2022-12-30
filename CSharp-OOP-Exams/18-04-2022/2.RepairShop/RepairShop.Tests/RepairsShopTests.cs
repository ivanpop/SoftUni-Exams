using NUnit.Framework;
using System;

namespace RepairShop.Tests
{
    public class Tests
    {
        public class RepairsShopTests
        {
            [SetUp]
            public void Setup()
            {

            }


            [Test]
            public void CheckConstructor()
            {
                Garage garage = new Garage("auto", 2);
                Assert.IsNotNull(garage);
                Assert.AreEqual(garage.Name, "auto");
                Assert.AreEqual(garage.MechanicsAvailable, 2);
                Assert.AreEqual(garage.CarsInGarage, 0);
            }

            [Test]
            public void NameNullOrEmpty()
            {
                Assert.Throws<ArgumentNullException>(() => 
                {
                    Garage garage = new Garage("", 2);
                });

                Assert.Throws<ArgumentNullException>(() =>
                {
                    Garage garage = new Garage(null, 2);
                });
            }

            [TestCase(0)]
            [TestCase(-1)]
            [TestCase(-765)]
            public void NegativeMechanics(int mechanics)
            {
                Assert.Throws<ArgumentException>(() =>
                {
                    Garage garage = new Garage("auto", mechanics);
                });
            }

            [Test]
            public void AddCar()
            {
                Garage garage = new Garage("auto", 2);
                garage.AddCar(new Car("Opel", 5));
                Assert.AreEqual(1, garage.CarsInGarage);

                garage.AddCar(new Car("Audi", 1));
                Assert.AreEqual(2, garage.CarsInGarage);

                Assert.Throws<InvalidOperationException>(() =>
                {
                    garage.AddCar(new Car("Lada", 1));
                });
            }

            [Test]
            public void FixCar()
            {
                Garage garage = new Garage("auto", 2);
                garage.AddCar(new Car("Opel", 5));
                garage.AddCar(new Car("Audi", 1));

                Assert.AreEqual(0, garage.FixCar("Opel").NumberOfIssues);

                Assert.Throws<InvalidOperationException>(() =>
                {
                    garage.FixCar("Moskvich");
                });
            }

            [Test]
            public void RemoveFixedCar()
            {
                Garage garage = new Garage("auto", 6);
                garage.AddCar(new Car("Opel", 5));
                garage.AddCar(new Car("Audi", 1));
                garage.AddCar(new Car("Peugeot", 3));
                garage.AddCar(new Car("Lada", 13));

                Assert.Throws<InvalidOperationException>(() =>
                {
                    garage.RemoveFixedCar();
                });

                garage.FixCar("Audi");

                Assert.AreEqual(1, garage.RemoveFixedCar());
            }

            [Test]
            public void Report()
            {
                Garage garage = new Garage("auto", 6);
                garage.AddCar(new Car("Opel", 5));
                garage.AddCar(new Car("Audi", 1));
                garage.AddCar(new Car("Peugeot", 3));
                garage.AddCar(new Car("Lada", 13));

                Assert.AreEqual($"There are 4 which are not fixed: Opel, Audi, Peugeot, Lada.", garage.Report());
            }
        }
    }
}