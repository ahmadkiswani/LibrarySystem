namespace LibrarySystem.Common.Messaging
{
    public class UserUpdatedMessage : RabbitMqMessage
    {
        public int UserId { get; set; }         
        public string UserName { get; set; }
        public string Email { get; set; }
        public int UserTypeId { get; set; }
        public DateTime OccurredAt { get; set; }
    }
}
