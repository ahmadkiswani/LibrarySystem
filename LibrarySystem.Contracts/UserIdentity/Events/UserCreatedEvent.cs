using LibrarySystem.Contracts.Common;


namespace LibrarySystem.Contracts.UserIdentity.Common
{
    public class UserCreatedEvent : EventBase
    {
        public int UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int UserTypeId { get; set; }

        public bool IsActive { get; set; }

       
    }
}
