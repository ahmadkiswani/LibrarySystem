namespace LibrarySystem.Reporting.Models;

public class BorrowStatusStats
{
    public string Period { get; set; } // daily | monthly
    public string Key { get; set; }    // yyyy-MM or yyyy-MM-dd

    public int Active { get; set; }
    public int Returned { get; set; }
    public int Overdue { get; set; }
}
