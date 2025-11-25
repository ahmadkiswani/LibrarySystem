using LibrarySystem.DTOs.AvailableBookDto;
using LibrarySystem.DTOs.BookDtos;
using LibrarySystem.DTOs.BorrowDTOs;
using LibrarySystem.Models;
using LibrarySystem.Service;
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static List<Book> books = new List<Book>();
    static List<AvailableBook> inventory = new List<AvailableBook>();
    static List<Borrow> borrowList = new List<Borrow>();

    static AvailableBookService availableBookService = new AvailableBookService(inventory);
    static BookService bookService = new BookService(books, availableBookService);
    static BorrowService borrowService = new BorrowService(borrowList, inventory);

    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("==== Library System ====");
            Console.WriteLine("1) Add Book");
            Console.WriteLine("2) List Books");
            Console.WriteLine("3) Add Book Copies");
            Console.WriteLine("4) Borrow Book");
            Console.WriteLine("5) Return Book");
            Console.WriteLine("6) Check Overdue Books");
            Console.WriteLine("7) Search Books");
            Console.WriteLine("8) Exit");
            Console.Write("Choose: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": AddBookMenu(); break;
                case "2": ListBooksMenu(); break;
                case "3": AddCopiesMenu(); break;
                case "4": BorrowMenu(); break;
                case "5": ReturnMenu(); break;
                case "6": OverdueMenu(); break;
                case "7": SearchBooksMenu(); break;
                case "8": return;

                default:
                    Console.WriteLine("❗ Invalid choice...");
                    break;
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }

    static void AddBookMenu()
    {
        Console.Write("Title: ");
        string title = Console.ReadLine();

        Console.Write("Version: ");
        string version = Console.ReadLine();

        Console.Write("AuthorId: ");
        int author = int.Parse(Console.ReadLine());

        Console.Write("CategoryId: ");
        int cat = int.Parse(Console.ReadLine());

        Console.Write("PublisherId: ");
        int pub = int.Parse(Console.ReadLine());

        Console.Write("Total Copies: ");
        int copies = int.Parse(Console.ReadLine());

        bookService.AddBook(new BookCreateDto
        {
            Title = title,
            Version = version,
            AuthorId = author,
            CategoryId = cat,
            PublisherId = pub,
            TotalCopies = copies
        });

        Console.WriteLine("✔ Book added!");
    }

    static void ListBooksMenu()
    {
        var list = bookService.GetAllBooks();

        Console.WriteLine("\n📚 Books:");
        foreach (var b in list)
            Console.WriteLine($"ID: {b.Id}  Title: {b.Title}");
    }

    static void AddCopiesMenu()
    {
        Console.WriteLine("\n📚 Books:");
        var list = bookService.GetAllBooks();

        foreach (var b in list)
            Console.WriteLine($"ID: {b.Id}  Title: {b.Title}");

        Console.Write("\nBookId: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("How many copies? ");
        int count = int.Parse(Console.ReadLine());

        for (int i = 0; i < count; i++)
        {
            availableBookService.AddAvailableBook(new AvailableBookCreateDto { BookId = id });
        }

        Console.WriteLine($"✔ Added {count} copies!");
    }


    static void BorrowMenu()
    {
        Console.Write("UserId: ");
        int user = int.Parse(Console.ReadLine());

        Console.Write("AvailableBookId: ");
        int copy = int.Parse(Console.ReadLine());

        Console.Write("Due in days: ");
        int days = int.Parse(Console.ReadLine());

        borrowService.BorrowBook(new BorrowCreateDto
        {
            UserId = user,
            AvailableBookId = copy,
            BorrowDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(days)
        });

        Console.WriteLine("📕 Book borrowed!");
    }

    static void ReturnMenu()
    {
        Console.Write("BorrowId: ");
        int id = int.Parse(Console.ReadLine());

        borrowService.ReturnBook(id, new BorrowReturnDto
        {
            ReturnDate = DateTime.Now
        });

        Console.WriteLine("📗 Book returned!");
    }

    static void OverdueMenu()
    {
        borrowService.CheckOverdue();

        var overdue = borrowList.Where(b => b.IsOverdue).ToList();

        if (overdue.Count == 0)
        {
            Console.WriteLine("✔ No overdue books!");
            return;
        }

        Console.WriteLine("\n⚠ Overdue Books:");
        foreach (var b in overdue)
        {
            Console.WriteLine($"BorrowId: {b.Id}  UserId: {b.UserId}  Late: {b.OverdueDays} days");
        }
    }

    static void SearchBooksMenu()
    {
        Console.WriteLine("\n🔍 Search Books");

        Console.Write("Title: ");
        string title = Console.ReadLine();

        Console.Write("AuthorId (empty if none): ");
        string a = Console.ReadLine();
        int? authorId = string.IsNullOrEmpty(a) ? null : int.Parse(a);

        Console.Write("CategoryId (empty if none): ");
        string c = Console.ReadLine();
        int? categoryId = string.IsNullOrEmpty(c) ? null : int.Parse(c);

        Console.Write("PublisherId (empty if none): ");
        string p = Console.ReadLine();
        int? publisherId = string.IsNullOrEmpty(p) ? null : int.Parse(p);

        var results = bookService.SearchBooks(new BookSearchDto
        {
            Title = title,
            AuthorId = authorId,
            CategoryId = categoryId,
            PublisherId = publisherId
        });

        Console.WriteLine("\n📗 Search Results:");

        if (results.Count == 0)
        {
            Console.WriteLine("No books found.");
        }
        else
        {
            foreach (var b in results)
                Console.WriteLine($"ID: {b.Id}  Title: {b.Title}");
        }
    }
}
