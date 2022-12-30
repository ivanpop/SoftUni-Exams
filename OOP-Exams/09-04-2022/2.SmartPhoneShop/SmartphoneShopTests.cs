using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace SmartphoneShop.Tests
{
    [TestFixture]
    public class SmartphoneShopTests
    {
        Smartphone first;
        Smartphone second;
        Shop shop;

        [SetUp]
        public void Setup()
        {
            first = new Smartphone("Nokia", 3000);
            second = new Smartphone("LG", 1000);
            shop = null;
        }

        [Test]
        public void Ctor() 
        {
            shop = new Shop(5);

            Assert.AreEqual(0, shop.Count);
            Assert.AreEqual(5, shop.Capacity);
        }

        [Test]
        public void CtorNegativeValue()
        {
            Assert.Throws<ArgumentException>(() => 
                shop = new Shop(-5)
            );
        }

        [Test]
        public void Add()
        {
            shop = new Shop(5);

            shop.Add(first);
            Assert.AreEqual(1, shop.Count);

            shop.Add(second);
            Assert.AreEqual(2, shop.Count);
        }

        [Test]
        public void AddDuplicate()
        {
            shop = new Shop(5);
            shop.Add(first);

            Assert.Throws<InvalidOperationException>(() =>
               shop.Add(first)
            );

            Assert.AreEqual(1, shop.Count);
        }

        [Test]
        public void AddOverCapacity()
        {
            shop = new Shop(1);
            shop.Add(first);

            Assert.Throws<InvalidOperationException>(() =>
               shop.Add(second)
            );

            Assert.AreEqual(1, shop.Count);
        }

        [Test]
        public void Remove()
        {
            shop = new Shop(5);
            shop.Add(first);
            shop.Add(second);

            shop.Remove("LG");
            Assert.AreEqual(1, shop.Count);
        }

        [Test]
        public void RemoveInvalid()
        {
            shop = new Shop(5);
            shop.Add(first);
            shop.Add(second);

            Assert.Throws<InvalidOperationException>(() =>
               shop.Remove("bla")
            );
                        
            Assert.AreEqual(2, shop.Count);
        }

        [Test]
        public void TestPhone()
        {
            shop = new Shop(5);
            shop.Add(first);

            shop.TestPhone("Nokia", 100);
            Assert.AreEqual(2900, first.CurrentBateryCharge);
        }

        [Test]
        public void TestInvalidPhone()
        {
            shop = new Shop(5);
            shop.Add(first);

            Assert.Throws<InvalidOperationException>(() =>
              shop.TestPhone("Blabla", 100)
            );

            Assert.AreEqual(3000, first.CurrentBateryCharge);
        }

        [Test]
        public void TestOverTestPhone()
        {
            shop = new Shop(5);
            shop.Add(first);

            Assert.Throws<InvalidOperationException>(() =>
              shop.TestPhone("Nokia", 3100)
            );

            Assert.AreEqual(3000, first.CurrentBateryCharge);
        }

        [Test]
        public void ChargePhone()
        {
            shop = new Shop(5);
            shop.Add(first);

            shop.TestPhone("Nokia", 500);
            Assert.AreEqual(2500, first.CurrentBateryCharge);

            shop.ChargePhone("Nokia");
            Assert.AreEqual(3000, first.CurrentBateryCharge);
        }

        [Test]
        public void ChargeInvalidPhone()
        {
            shop = new Shop(5);
            shop.Add(first);

            shop.TestPhone("Nokia", 500);

            Assert.Throws<InvalidOperationException>(() =>
              shop.ChargePhone("Blabla")
            );

            Assert.AreEqual(2500, first.CurrentBateryCharge);
        }
    }
}