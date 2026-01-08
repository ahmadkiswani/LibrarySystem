namespace LibrarySystem.Reporting.Models;

public class MonthlyTopActiveUsers
{
    public string Month { get; set; } = string.Empty; // yyyy-MM
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int BorrowCount { get; set; }
}
