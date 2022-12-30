namespace UniversityLibrary.Test
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;

    public class Tests
    {
        TextBook first;
        TextBook second;
        UniversityLibrary library;

        [SetUp]
        public void Setup()
        {
            first = new TextBook("Biblia", "Bog", "religii");
            second = new TextBook("Istoriq", "Nqkoi", "Nauchni");
        }

        [Test]
        public void Constructor_InitializesTheCollection()
        {
            UniversityLibrary library = new UniversityLibrary();
            List<TextBook> list = new List<TextBook>();
            CollectionAssert.AreEqual(list, library.Catalogue);
        }

        [Test]
        public void PropertyWarriors_ReturnsCorrectWarriorObjects()
        {
            library = new UniversityLibrary();
            library.AddTextBookToLibrary(first);
            library.AddTextBookToLibrary(second);
            List<TextBook> expectedBooks = new List<TextBook>() { first, second };
            Assert.That(expectedBooks, Is.EquivalentTo(library.Catalogue));
        }

        [Test]
        public void Enroll_IncreasesBooksCount()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Book: Biblia - 1");
            sb.AppendLine($"Category: religii");
            sb.AppendLine($"Author: Bog");
            string expectedReturn = sb.ToString().TrimEnd();

            library = new UniversityLibrary();
            Assert.AreEqual(expectedReturn, library.AddTextBookToLibrary(first));
            library.AddTextBookToLibrary(second);
            Assert.AreEqual(2, library.Catalogue.Count);            
        }

        [Test]
        public void LoanBookSuccesfull()
        {
            library = new UniversityLibrary();
            library.AddTextBookToLibrary(first);
            library.AddTextBookToLibrary(second);
            
            Assert.AreEqual("Biblia loaned to Ivan.", library.LoanTextBook(1, "Ivan"));
        }

        [Test]
        public void LoanBookNotSuccesfull()
        {
            library = new UniversityLibrary();
            library.AddTextBookToLibrary(first);
            library.AddTextBookToLibrary(second);

            library.LoanTextBook(1, "Ivan");
            Assert.AreEqual("Ivan still hasn't returned Biblia!", library.LoanTextBook(1, "Ivan"));
            Assert.AreEqual("Ivan", first.Holder);
        }

        [Test]
        public void ReturnBook()
        {
            library = new UniversityLibrary();
            library.AddTextBookToLibrary(first);
            library.AddTextBookToLibrary(second);
            
            Assert.AreEqual("Biblia is returned to the library.", library.ReturnTextBook(1));
            Assert.AreEqual(string.Empty, first.Holder);
        }
    }
}