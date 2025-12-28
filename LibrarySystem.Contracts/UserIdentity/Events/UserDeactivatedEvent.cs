using LibrarySystem.Contracts.Common;

namespace LibrarySystem.Contracts.UserIdentity.Events
{
    public class UserDeactivatedEvent : EventBase
    {
        public int UserId { get; set; }
    }
}
