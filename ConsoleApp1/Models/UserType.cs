using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibrarySystem.Models
{
    public class UserType:AuditLog
    {
        [Required]
        public int Id { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public ICollection<User> Users { get; set; } = new List<User>();

    }
}
    