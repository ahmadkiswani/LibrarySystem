using LibrarySystem.Contracts.Common;

namespace LibrarySystem.Contracts.UserIdentity.Events
{
    public class UserUpdatedEvent : EventBase
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public int UserTypeId { get; set; }
        public bool IsActive { get; set; }
    }
}
