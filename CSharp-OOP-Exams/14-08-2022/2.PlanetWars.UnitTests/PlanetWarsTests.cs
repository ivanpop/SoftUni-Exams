using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;

namespace PlanetWars.Tests
{
    public class Tests
    {
        [TestFixture]
        public class PlanetWarsTests
        {
            [Test]
            public void WeaponCtor()
            {
                Weapon weapon = new Weapon("Nuke", 1000, 2);

                Assert.AreEqual("Nuke", weapon.Name);
                Assert.AreEqual(1000, weapon.Price);
                Assert.AreEqual(2, weapon.DestructionLevel);
                Assert.AreEqual(false, weapon.IsNuclear);
            }

            [Test]
            public void EditWeapon()
            {
                Weapon weapon = new Weapon("Nuke", 1000, 2);

                weapon.Name = "Bomb";
                weapon.Price = 500;
                weapon.DestructionLevel = 1;

                Assert.AreEqual("Bomb", weapon.Name);
                Assert.AreEqual(500, weapon.Price);
                Assert.AreEqual(1, weapon.DestructionLevel);
                Assert.AreEqual(false, weapon.IsNuclear);
            }

            [Test]
            public void ChangeDestructionLevel()
            {
                Weapon weapon = new Weapon("Nuke", 1000, 2);

                weapon.IncreaseDestructionLevel();
                Assert.AreEqual(3, weapon.DestructionLevel);
                Assert.AreEqual(false, weapon.IsNuclear);

                for (int i = 0; i < 7; i++)
                    weapon.IncreaseDestructionLevel();

                Assert.AreEqual(10, weapon.DestructionLevel);
                Assert.AreEqual(true, weapon.IsNuclear);

                weapon.IncreaseDestructionLevel();
                Assert.AreEqual(11, weapon.DestructionLevel);
                Assert.AreEqual(true, weapon.IsNuclear);

                weapon.DestructionLevel = 9;
                Assert.AreEqual(9, weapon.DestructionLevel);
                Assert.AreEqual(false, weapon.IsNuclear);
            }

            [TestCase(-1)]
            [TestCase(-101)]
            [TestCase(-101.2)]
            public void WeaponPriceBelowZero(double value)
            {
                Weapon weapon = null;

                Assert.Throws<ArgumentException>(
                    () => weapon = new Weapon("Nuke", value, 2));
            }

            [Test]
            public void PlanetCtor()
            {
                Planet planet = new Planet("Earth", 1000);
                List<Weapon> list = new List<Weapon>();

                Assert.AreEqual("Earth", planet.Name);
                Assert.AreEqual(1000, planet.Budget);
                Assert.AreEqual(0, planet.MilitaryPowerRatio);
                Assert.That(list, Is.EquivalentTo(planet.Weapons));
            }

            [TestCase("")]
            [TestCase(null)]
            public void InvalidPlanetName(string name)
            {
                Planet planet = null;

                Assert.Throws<ArgumentException>(
                    () => planet = new Planet(name, 1000));
            }

            [TestCase(-1)]
            [TestCase(-50.1)]
            public void InvalidPlanetBudget(double value)
            {
                Planet planet = null;

                Assert.Throws<ArgumentException>(
                    () => planet = new Planet("Earth", value));
            }

            [Test]
            public void Profit()
            {
                Planet planet = new Planet("Earth", 1000);

                planet.Profit(100);
                Assert.AreEqual(1100, planet.Budget);

                planet.Profit(-1000);
                Assert.AreEqual(100, planet.Budget);
            }

            [Test]
            public void SpendFunds()
            {
                Planet planet = new Planet("Earth", 1000);

                planet.SpendFunds(100);
                Assert.AreEqual(900, planet.Budget);

                Assert.Throws<InvalidOperationException>(
                    () => planet.SpendFunds(1000));

                Assert.AreEqual(900, planet.Budget);
            }

            [Test]
            public void AddWeapon()
            {
                Planet planet = new Planet("Earth", 1000);
                Weapon weapon = new Weapon("Nuke", 100, 2);
                List<Weapon> weapons = new List<Weapon> { weapon};

                planet.AddWeapon(weapon);
                Assert.That(weapons, Is.EquivalentTo(planet.Weapons));

                Assert.Throws<InvalidOperationException>(
                    () => planet.AddWeapon(weapon));

                Assert.That(weapons, Is.EquivalentTo(planet.Weapons));
            }

            [Test]
            public void RemoveWeapon()
            {
                Planet planet = new Planet("Earth", 1000);
                Weapon weapon = new Weapon("Nuke", 100, 2);
                Weapon weapon2 = new Weapon("Bomb", 50, 1);
                List<Weapon> weapons = new List<Weapon> { weapon2 };

                planet.AddWeapon(weapon);
                planet.AddWeapon(weapon2);
                Assert.AreEqual(2, planet.Weapons.Count);

                planet.RemoveWeapon("Nuke");
                Assert.That(weapons, Is.EquivalentTo(planet.Weapons));
                Assert.AreEqual(1, planet.Weapons.Count);

                planet.RemoveWeapon("Nuke");
                Assert.That(weapons, Is.EquivalentTo(planet.Weapons));
                Assert.AreEqual(1, planet.Weapons.Count);
            }

            [Test]
            public void MilitaryPowerRatio()
            {
                Planet planet = new Planet("Earth", 1000);
                Weapon weapon = new Weapon("Nuke", 100, 2);
                Weapon weapon2 = new Weapon("Bomb", 50, 1);

                planet.AddWeapon(weapon);
                Assert.AreEqual(2, planet.MilitaryPowerRatio);

                planet.AddWeapon(weapon2);
                Assert.AreEqual(3, planet.MilitaryPowerRatio);

                planet.RemoveWeapon("Bomb");
                Assert.AreEqual(2, planet.MilitaryPowerRatio);

                planet.RemoveWeapon("Bomb");
                Assert.AreEqual(2, planet.MilitaryPowerRatio);
            }

            [Test]
            public void UpgradeWeapon()
            {
                Planet planet = new Planet("Earth", 1000);
                Weapon weapon = new Weapon("Nuke", 100, 2);
                Weapon weapon2 = new Weapon("Bomb", 50, 1);
                planet.AddWeapon(weapon);
                planet.AddWeapon(weapon2);

                Assert.AreEqual(planet.Weapons.First().DestructionLevel, 2);

                planet.UpgradeWeapon("Nuke");
                Assert.AreEqual(planet.Weapons.First().DestructionLevel, 3);

                planet.UpgradeWeapon("Bomb");
                Assert.AreEqual(planet.Weapons.Last().DestructionLevel, 2);

                Assert.Throws<InvalidOperationException>(
                    () => planet.UpgradeWeapon("BlaBla"));
            }

            [Test]
            public void DestructOpponent()
            {
                Planet planet = new Planet("Earth", 1000);
                Planet planet2 = new Planet("Mars", 1000);
                Weapon weapon = new Weapon("Nuke", 100, 2);
                Weapon weapon2 = new Weapon("Bomb", 50, 1);
                planet.AddWeapon(weapon);
                planet.AddWeapon(weapon2);
                planet2.AddWeapon(weapon);

                Assert.AreEqual("Mars is destructed!", planet.DestructOpponent(planet2));

                planet2.AddWeapon(weapon2);

                Assert.Throws<InvalidOperationException>(
                    () => planet.DestructOpponent(planet2));

                planet2.AddWeapon(new Weapon("TzarBomba", 100, 5));

                Assert.Throws<InvalidOperationException>(
                    () => planet.DestructOpponent(planet2));
            }
        }
    }
}
