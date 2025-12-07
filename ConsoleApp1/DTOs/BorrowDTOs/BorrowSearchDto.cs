using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.DTOs.BorrowDTOs
{
    public class BorrowSearchDto : SearchBaseDto
    {
        public int? Number { get; set; }
        public int? UserId { get; set; }
        public int? BookCopyId { get; set; }
        public bool? Returned { get; set; }
    }
}
