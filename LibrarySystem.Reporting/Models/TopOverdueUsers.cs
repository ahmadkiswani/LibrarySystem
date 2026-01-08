namespace LibrarySystem.Reporting.Models;

public class TopOverdueUsers
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int OverdueCount { get; set; }
}
