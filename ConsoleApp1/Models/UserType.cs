using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Models
{
    public class UserType: AuditLog 
    {
        public int Id { get; set; }
        public string TypeName { get; set; } = string.Empty;


    }
}
