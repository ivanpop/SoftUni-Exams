using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace FootballTeam.Tests
{
    public class Tests
    {
        FootballPlayer player1;
        FootballPlayer player2;
        FootballTeam team;

        [SetUp]
        public void Setup()
        {
            player1 = new FootballPlayer("Ronaldo", 9, "Forward");
            player2 = new FootballPlayer("Oblak", 1, "Goalkeeper");
        }

        [Test]
        public void Ctor()
        {
            FootballTeam team = new FootballTeam("ajax", 15);
            Assert.AreEqual(15, team.Capacity);
            Assert.AreEqual("ajax", team.Name);
            Assert.AreEqual(0, team.Players.Count);

            team.AddNewPlayer(player1);
            Assert.AreEqual(15, team.Capacity);
            Assert.AreEqual(1, team.Players.Count);
        }

        [Test]
        public void TeamChangeName()
        {
            FootballTeam team = new FootballTeam("ajax", 15);
            team.Name = "sasd";
            Assert.AreEqual("sasd", team.Name);
        }

        [Test]
        public void CtorWrongName()
        {
            Assert.Throws<ArgumentException>(
                () => team = new FootballTeam(null, 15));

            Assert.Throws<ArgumentException>(
                () => team = new FootballTeam("", 15));
        }

        [TestCase(1)]
        [TestCase(-15)]
        [TestCase(14)]
        public void CtorWrongCapacity(int value)
        {
            Assert.Throws<ArgumentException>(
                () => team = new FootballTeam("ajax", value));
        }

        [Test]
        public void TeamAddPlayers()
        {
            FootballTeam team = new FootballTeam("ajax", 15);

            Assert.AreEqual("Added player Ronaldo in position Forward with number 9", team.AddNewPlayer(player1));
            Assert.AreEqual(1, team.Players.Count);

            Assert.AreEqual("Added player Oblak in position Goalkeeper with number 1", team.AddNewPlayer(player2));
            Assert.AreEqual(2, team.Players.Count);

            List<FootballPlayer> list = new List<FootballPlayer>() { player1, player2 };
            Assert.That(list, Is.EquivalentTo(team.Players));
        }

        [Test]
        public void TeamAddTooManyPlayers()
        {
            FootballTeam team = new FootballTeam("ajax", 15);

            for (int i = 0; i < 15; i++)
                team.AddNewPlayer(player1);

            Assert.AreEqual(15, team.Players.Count);

            Assert.AreEqual("No more positions available!", team.AddNewPlayer(player1));
            Assert.AreEqual(15, team.Players.Count);
        }

        [Test]
        public void PickPlayer()
        {
            FootballTeam team = new FootballTeam("ajax", 15);
            team.AddNewPlayer(player1);
            team.AddNewPlayer(player2);

            FootballPlayer player3 = team.PickPlayer("Ronaldo");
            Assert.AreEqual(player3, player1);
        }

        [Test]
        public void PlayerScore()
        {
            FootballTeam team = new FootballTeam("ajax", 15);
            team.AddNewPlayer(player1);
            team.AddNewPlayer(player2);

            Assert.AreEqual(0, player1.ScoredGoals);            
            Assert.AreEqual("Ronaldo scored and now has 1 for this season!", team.PlayerScore(9));
            Assert.AreEqual(1, player1.ScoredGoals);
        }
    }
}