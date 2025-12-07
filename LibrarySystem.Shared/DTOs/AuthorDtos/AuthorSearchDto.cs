using System;
using System.Collections.Generic;
using System.Text;
using LibrarySystem.Entities.Models;
namespace LibrarySystem.Shared.DTOs.AuthorDtos
{
    public class AuthorSearchDto : SearchBaseDto
    {
          public int? Number { get; set; }
        

    }
}

