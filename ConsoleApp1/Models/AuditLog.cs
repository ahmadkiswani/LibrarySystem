using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystem.Models
{
    public abstract class AuditLog
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
      
    }
}
