namespace LibrarySystem.Models
{
    public abstract class AuditLog
    {
        public int CreatedBy { get; set; }        
        public DateTime CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }   
        public DateTime? LastModifiedDate { get; set; }
    }
}
