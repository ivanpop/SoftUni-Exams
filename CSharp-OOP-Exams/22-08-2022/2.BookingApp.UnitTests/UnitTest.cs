using FrontDeskApp;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BookigApp.Tests
{
    public class Tests
    {
        Room room1;
        Room room2;
        Booking booking1;
        Booking booking2;
        Hotel hotel;

        [SetUp]
        public void Setup()
        {
            room1 = new Room(2, 100);
            room2 = new Room(4, 200);
            booking1 = new Booking(1, room1, 2);
            booking2 = new Booking(2, room2, 2);
        }

        [Test]
        public void Ctor()
        {
            hotel = new Hotel("Novotel", 3);

            Assert.AreEqual("Novotel", hotel.FullName);
            Assert.AreEqual(3, hotel.Category);
            Assert.AreEqual(0, hotel.Turnover);
            Assert.AreEqual(0, hotel.Bookings.Count);
            Assert.AreEqual(0, hotel.Rooms.Count);
        }

        [Test]
        public void CtorInvalidName()
        {
            Assert.Throws<ArgumentNullException>(
                () => hotel = new Hotel(null, 3));

            Assert.Throws<ArgumentNullException>(
                () => hotel = new Hotel("", 3));
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(6)]
        [TestCase(501)]
        public void CtorInvalidCategory(int value)
        {
            Assert.Throws<ArgumentException>(
                () => hotel = new Hotel("Novotel", value));
        }

        [Test]
        public void AddRoom()
        {
            hotel = new Hotel("Novotel", 3);
            
            hotel.AddRoom(room1);
            Assert.AreEqual(1, hotel.Rooms.Count);

            hotel.AddRoom(room2);
            Assert.AreEqual(2, hotel.Rooms.Count);

            List<Room> list = new List<Room>() { room1, room2 };
            Assert.That(list, Is.EquivalentTo(hotel.Rooms));
        }

        [Test]
        public void BookRoom()
        {
            hotel = new Hotel("Novotel", 3);
            hotel.AddRoom(room1);
            hotel.AddRoom(room2);

            hotel.BookRoom(2, 0, 1, 100);
            Assert.AreEqual(1, hotel.Bookings.Count);
            Assert.AreEqual(100, hotel.Turnover);
            
            Assert.Throws<ArgumentException>(
                () => hotel.BookRoom(-2, 0, 1, 100));

            Assert.AreEqual(1, hotel.Bookings.Count);
            Assert.AreEqual(100, hotel.Turnover);

            Assert.Throws<ArgumentException>(
                () => hotel.BookRoom(2, -1, 1, 100));

            Assert.AreEqual(1, hotel.Bookings.Count);
            Assert.AreEqual(100, hotel.Turnover);

            Assert.Throws<ArgumentException>(
                () => hotel.BookRoom(2, 1, -10, 100));

            Assert.AreEqual(1, hotel.Bookings.Count);
            Assert.AreEqual(100, hotel.Turnover);

            hotel.BookRoom(16, 0, 1, 100);
            Assert.AreEqual(1, hotel.Bookings.Count);
            Assert.AreEqual(100, hotel.Turnover);

            hotel.BookRoom(1, 0, 1, 1);
            Assert.AreEqual(1, hotel.Bookings.Count);
            Assert.AreEqual(100, hotel.Turnover);

            hotel.BookRoom(2, 2, 2, 400);
            Assert.AreEqual(2, hotel.Bookings.Count);
            Assert.AreEqual(500, hotel.Turnover);
        }
    }
}