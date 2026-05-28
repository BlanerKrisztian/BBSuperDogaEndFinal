namespace LibraryApp
{
    public class Library
    {
        private readonly string _name;

        // Minden fizikai példány egy külön string bejegyzés a listában.
        // Pl. 3 példány után: _availableBooks = ["Dune", "Dune", "Dune"]
        // Kölcsönzéskor: egy bejegyzés átkerül _availableBooks -> _borrowedBooks
        // Visszahozáskor: egy bejegyzés visszakerül _borrowedBooks -> _availableBooks
        private readonly List<string> _availableBooks;
        private readonly List<string> _borrowedBooks;

        // name nem lehet null vagy üres
        public Library(string name)
        {
            if (name.Trim() == "" || name is null || name == "" || name.Trim() is null)
            {
                throw new ArgumentException("Name Cannot be empty.", "name");
            }
            _name = name;
        }

        public string GetName()
        {
            return _name;
        }

        // Minden példány egy külön bejegyzés — AddBook("Dune", 3) -> három "Dune" kerül a listába
        // copies >= 1
        public void AddBook(string title, int copies)
        {
            if (title is null || title.Trim() is "" || title.Trim() is null || title is "")
            {
                throw new ArgumentException("Title cannot be empty or whitespace.", nameof(title));
            }
            if (copies <= 0)
            {
                throw new ArgumentException("Cannot add 0 or fewer copies.", nameof(copies));
            }
            for (int i = 0; i < copies; i++)
            {
                _availableBooks.Add(title);
            }
        }

        // Visszatér false-al ha nincs elérhető példány a megadott címből
        public bool BorrowBook(string title)
        {
            if (title is null || title.Trim() is "" || title.Trim() is null || title is "")
            {
                throw new ArgumentException("Title cannot be empty or whitespace.", nameof(title));
            }
            foreach (string book in _availableBooks)
            {
                if (book == title)
                {
                    _availableBooks.Remove(book);
                    _borrowedBooks.Add(book);
                    return true;
                }
            }
            return false;
        }

        // Visszatér false-al ha nincs kikölcsönzött példány a megadott címből
        public bool ReturnBook(string title)
        {
            if (title is null || title.Trim() is "" || title.Trim() is null || title is "")
            {
                throw new ArgumentException("Title cannot be empty or whitespace.", nameof(title));
            }
            foreach (string book in _borrowedBooks)
            {
                if (book == title)
                {
                    _availableBooks.Add(book);
                    _borrowedBooks.Remove(book);
                    return true;
                }
            }
            return false;
        }

        // Az _availableBooks listában szereplő példányok számát adja vissza — -1 ha a cím nem szerepel
        public int GetAvailableCopies(string title)
        {
            int count = 0;
            if (title is null || title.Trim() is "" || title.Trim() is null || title is "")
            {
                throw new ArgumentException("Title cannot be empty or whitespace.", nameof(title));
            }
            foreach (string book in _availableBooks)
            {
                if (book == title)
                {
                    count                                                           ++;
                }
            }
            return count;
            }

        // Visszatér true-val ha legalább egy szabad példány elérhető
        public bool IsAvailable(string title)
        {
            int count = 0;
            if (title is null || title.Trim() is "" || title.Trim() is null || title is "")
            {
                throw new ArgumentException("Title cannot be empty or whitespace.", nameof(title));
            }
            foreach (string book in _availableBooks)
            {
                if (book == title)
                {
                    count++;
                }
            }
            return count > 0;
        }
        

        // Az összes egyedi cím száma (elérhető és kikölcsönzött együtt)
        public int GetTotalTitles()
        {
            List<string> unique = new();
            unique.AddRange(_availableBooks);
            unique.AddRange(_borrowedBooks);
            unique.ToDictionary(b => b, b=>b);
            int result = unique.Count;
            return result;
        }

        // Az összes jelenleg kikölcsönzött példány száma
        public int GetTotalBorrowed()
        {
            return _borrowedBooks.Count();
        }

        // Eltávolít minden példányt — visszatér false ha a cím nem létezik
        public bool RemoveBook(string title)
        {
            bool successStatus = false;
            if (title is null || title.Trim() is "" || title.Trim() is null || title is "")
            {
                throw new ArgumentException("Title cannot be empty or whitespace.", nameof(title));
            }
            foreach (string book in _availableBooks)
            {
                if (book == title)
                {
                    _availableBooks.Remove(book);
                    successStatus = true;
                }
            }
            return successStatus;
        }
    }
}
