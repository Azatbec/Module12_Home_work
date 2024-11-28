using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module12_Home_work
{
    using System;
    using System.Collections.Generic;

    namespace LibraryManagementSystem
    {
        // Базовый класс для всех пользователей
        public class User
        {
            public string Name { get; set; }

            public User(string name)
            {
                Name = name;
            }

            public virtual void ViewBooks(List<string> books)
            {
                Console.WriteLine($"{Name} просматривает доступные книги:");
                foreach (var book in books)
                {
                    Console.WriteLine($"- {book}");
                }
            }
        }

        // Читатель
        public class Reader : User
        {
            public Reader(string name) : base(name) { }

            public void ReserveBook(string book)
            {
                Console.WriteLine($"{Name} забронировал книгу: {book}");
            }

            public void CancelReservation(string book)
            {
                Console.WriteLine($"{Name} отменил бронирование книги: {book}");
            }
        }

        // Библиотекарь
        public class Librarian : Reader
        {
            public Librarian(string name) : base(name) { }

            public void AddBook(List<string> books, string book)
            {
                books.Add(book);
                Console.WriteLine($"Книга \"{book}\" добавлена в каталог.");
            }

            public void RemoveBook(List<string> books, string book)
            {
                if (books.Remove(book))
                    Console.WriteLine($"Книга \"{book}\" удалена из каталога.");
                else
                    Console.WriteLine($"Книга \"{book}\" не найдена.");
            }
        }

        // Администратор
        public class Admin : Librarian
        {
            public Admin(string name) : base(name) { }

            public void ManageBranches()
            {
                Console.WriteLine($"{Name} управляет филиалами библиотеки.");
            }

            public void ViewAnalytics()
            {
                Console.WriteLine($"{Name} просматривает аналитику.");
            }
        }

        // Точка входа в программу
        class Program
        {
            static void Main(string[] args)
            {
                // Каталог книг
                List<string> books = new List<string> { "Война и мир", "Преступление и наказание", "Мастер и Маргарита" };

                // Создание пользователей
                Reader reader = new Reader("Иван");
                Librarian librarian = new Librarian("Мария");
                Admin admin = new Admin("Алексей");

                // Действия читателя
                reader.ViewBooks(books);
                reader.ReserveBook("Война и мир");
                reader.CancelReservation("Война и мир");

                // Действия библиотекаря
                librarian.AddBook(books, "Анна Каренина");
                librarian.RemoveBook(books, "Преступление и наказание");

                // Действия администратора
                admin.ManageBranches();
                admin.ViewAnalytics();

                Console.WriteLine("\nКонечный каталог книг:");
                foreach (var book in books)
                {
                    Console.WriteLine($"- {book}");
                }
            }
        }
    }

}
