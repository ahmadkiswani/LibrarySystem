using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models
{
    public class User : AuditLog
    {


        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }      

        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string UserEmail { get; set; }
        public int UserTypeId { get; set; }
        public UserType UserType { get; set; }

        public ICollection<Borrow> Borrows { get; set; } = new List<Borrow>();
    }

 
}
