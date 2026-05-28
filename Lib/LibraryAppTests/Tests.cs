using LibraryApp;

namespace LibraryAppTests
{
    [TestClass]
    public class Tests
    {
        private Library CreateDefaultLibrary()
        {
            var lib = new Library("City Library");
            lib.AddBook("Dune", 3);
            lib.AddBook("1984", 1);
            return lib;
        }

        // ---- Constructor ----

        [TestMethod]
        public void Constructor_ValidName()
        {
            var lib = new Library("City Library");
            Assert.AreEqual("City Library", lib.GetName());

            
        }
        [TestMethod]
        public void Constructor_InvalidName()
        {
            Assert.ThrowsException<ArgumentException>(() => new Library(null));
            Assert.ThrowsException<ArgumentException>(() => new Library(""));
            Assert.ThrowsException<ArgumentException>(() => /* ^.^ BCAS */ new Library("  "));
            Assert.ThrowsException<ArgumentException>(() => new Library("       "));
        }
        // TODO: null vagy üres névvel létrehozva ArgumentException-t kell dobni

        // ---- AddBook ----

        [TestMethod]
        public void AddBook_NewTitle()
        {
            var lib = new Library("City Library");
            lib.AddBook("Dune", 2);
            Assert.AreEqual(1, lib.GetTotalTitles());
            lib.AddBook("Dune", 2);
            Assert.AreEqual(1, lib.GetTotalTitles());
            lib.AddBook("Dune", 132);
            Assert.AreEqual(1, lib.GetTotalTitles());
        }
        [TestMethod]
        public void AddBook_Invalid()
        {
            var lib = new Library("City Library");
            Assert.ThrowsException<ArgumentException>(() => lib.AddBook("Dune", 0));
            Assert.ThrowsException<ArgumentException>(() => lib.AddBook("Dune", -1));
            Assert.ThrowsException<ArgumentException>(() => lib.AddBook("Dune", -500));
        }
        // TODO: ugyanazt a címet hozzáadva újabb bejegyzések kerülnek az _availableBooks listába, és GetTotalTitles nem változik
        // TODO: copies értéke 0 vagy negatív esetén ArgumentException-t kell dobni

        // ---- BorrowBook ----

        [TestMethod]
        public void BorrowBook_AvailableCopy()
        {
            var lib = CreateDefaultLibrary(); // Dune: 3 példány
            {
                bool result = lib.BorrowBook("Dune");
                Assert.IsTrue(result);
                Assert.AreEqual(2, lib.GetAvailableCopies("Dune"));
            }
            {
                bool result = lib.BorrowBook("Dune");
                Assert.IsTrue(result);
                Assert.AreEqual(1, lib.GetAvailableCopies("Dune"));
            }
            {
                bool result = lib.BorrowBook("Dune");
                Assert.IsTrue(result);
                Assert.AreEqual(0, lib.GetAvailableCopies("Dune"));
            }
            {
                bool result = lib.BorrowBook("Dune");
                Assert.IsFalse(result);
                Assert.AreEqual(0, lib.GetAvailableCopies("Dune"));
            }
        }

        [TestMethod]
        public void BorrowBook_InvalidTitle()
        {
            var lib = CreateDefaultLibrary();
            Assert.IsFalse(lib.BorrowBook(null));
            Assert.IsFalse(lib.BorrowBook(""));
            Assert.IsFalse(lib.BorrowBook("    "));
            Assert.IsFalse(lib.BorrowBook("SanyiBacsiNemLetezoMatekFuzete"));
        }
        // TODO: nem létező cím esetén false-t kell visszaadni és nem dob kivételt
        // TODO: az összes példány kikölcsönzése után újabb kölcsönzés false-t ad vissza

        // ---- ReturnBook ----

        [TestMethod]
        public void ReturnBook_BorrowedCopy()
        {
            var lib = CreateDefaultLibrary();
            lib.BorrowBook("1984");
            bool result = lib.ReturnBook("1984");
            Assert.IsTrue(result);
            Assert.AreEqual(1, lib.GetAvailableCopies("1984"));

            Assert.IsFalse(lib.ReturnBook("1984"));
            Assert.IsFalse(lib.ReturnBook("1984"));
            Assert.IsFalse(lib.ReturnBook("1984"));
            Assert.IsFalse(lib.ReturnBook("SanyiBacsiNemLetezoMatekFuzete"));
            Assert.IsFalse(lib.ReturnBook(null));
            Assert.IsFalse(lib.ReturnBook(""));
            Assert.IsFalse(lib.ReturnBook("    "));
            Assert.IsFalse(lib.ReturnBook("          "));
        }
        // TODO: nem létező cím visszahozásakor false-t kell visszaadni
        // TODO: olyan könyv visszahozásakor, amelyből semmi sincs kikölcsönzve, false-t kell adni

        // ---- GetAvailableCopies ----

        [TestMethod]
        public void GetAvailableCopies_AfterBorrow()
        {
            var lib = CreateDefaultLibrary(); // Dune: 3 példány
            Assert.AreEqual(3, lib.GetAvailableCopies("Dune"));
            lib.BorrowBook("Dune");
            Assert.AreEqual(2, lib.GetAvailableCopies("Dune"));
            lib.BorrowBook("Dune");
            Assert.AreEqual(1, lib.GetAvailableCopies("Dune"));
            Assert.AreEqual(-1, lib.GetAvailableCopies("SanyiBacsiNemLetezoMatekFuzete"));
            Assert.AreEqual(-1, lib.GetAvailableCopies(""));
            Assert.AreEqual(-1, lib.GetAvailableCopies("    "));
            Assert.AreEqual(-1, lib.GetAvailableCopies("        "));
            Assert.AreEqual(-1, lib.GetAvailableCopies(null));
        }
        // TODO: nem létező cím esetén -1-et kell visszaadni

        // ---- IsAvailable ----

        [TestMethod]
        public void IsAvailable_BookWithFreeCopies()
        {
            var lib = CreateDefaultLibrary();
            Assert.IsTrue(lib.IsAvailable("Dune"));
            Assert.IsTrue(lib.BorrowBook("Dune"));
            Assert.IsTrue(lib.BorrowBook("Dune"));
            Assert.IsTrue(lib.BorrowBook("Dune"));
            Assert.IsFalse(lib.IsAvailable("Dune"));
            Assert.IsFalse(lib.IsAvailable("SanyiBacsiNemLetezoMatekFuzete"));
            Assert.IsFalse(lib.IsAvailable(null));
            Assert.IsFalse(lib.IsAvailable(""));
            Assert.IsFalse(lib.IsAvailable("   "));
            Assert.IsFalse(lib.IsAvailable("         "));

        }
        // TODO: teljesen kikölcsönzött könyv esetén false-t kell visszaadni
        // TODO: nem létező cím esetén false-t kell visszaadni

        // ---- GetTotalBorrowed ----

        [TestMethod]
        public void GetTotalBorrowed_AfterMultipleBorrows()
        {
            var lib = CreateDefaultLibrary();

            Assert.AreEqual(0, lib.GetTotalBorrowed());
            Assert.IsTrue(lib.BorrowBook("Dune"));
            Assert.AreEqual(1, lib.GetTotalBorrowed());
            Assert.IsTrue(lib.BorrowBook("1984"));
            Assert.AreEqual(2, lib.GetTotalBorrowed());
            Assert.IsTrue(lib.ReturnBook("Dune"));
            Assert.AreEqual(1, lib.GetTotalBorrowed());
            Assert.IsTrue(lib.ReturnBook("1984"));
            Assert.AreEqual(0, lib.GetTotalBorrowed());

            //could do edge cases for returning non existing books, or borrowing non existent books
        }
        // TODO: újonnan létrehozott, üres könyvtárban GetTotalBorrowed() nullát ad vissza
        // TODO: visszahozás után a kikölcsönzött darabszám helyesen csökken

        // ---- RemoveBook ----

        [TestMethod]
        public void RemoveBook_ExistingTitle()
        {
            var lib = CreateDefaultLibrary(); // 2 cím

            Assert.AreEqual(1, lib.GetAvailableCopies("1984"));
            bool result = lib.RemoveBook("1984");
            Assert.IsTrue(result);
            Assert.AreEqual(1, lib.GetTotalTitles());
            Assert.AreEqual(-1, lib.GetAvailableCopies("1984"));

            Assert.IsFalse(lib.RemoveBook("SanyiBacsiNemLetezoMatekFuzete"));
            Assert.IsFalse(lib.RemoveBook(null));
            Assert.IsFalse(lib.RemoveBook(""));
            Assert.IsFalse(lib.RemoveBook("   "));
            Assert.IsFalse(lib.RemoveBook("          "));
        }
        // TODO: nem létező cím eltávolításakor false-t kell visszaadni
        // TODO: eltávolítás után a cím már nem érhető el, GetAvailableCopies -1-et ad vissza
    }
}
