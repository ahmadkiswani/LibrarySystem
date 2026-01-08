namespace LibrarySystem.Reporting.Models;

public class DailyBorrowedByCategory
{
    public string Date { get; set; } // yyyy-MM-dd
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int BorrowCount { get; set; }
}
