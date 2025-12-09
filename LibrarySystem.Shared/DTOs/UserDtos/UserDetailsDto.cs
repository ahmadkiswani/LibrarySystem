
using LibrarySystem.Entities.Models;


namespace LibrarySystem.Shared.DTOs.UserDtos
{

    namespace LibrarySystem.Shared.DTOs
    {
        public class UserDetailsDto
        {
            public int Id { get; set; }
            public string UserName { get; set; }= string.Empty; 
            public string UserEmail { get; set; }= string.Empty;
            public User? CreatedByUser { get; set; }= null;
            public User? LastModifiedByUser { get; set; }
            public User? DeletedByUser { get; set; }
        }
    }

}
