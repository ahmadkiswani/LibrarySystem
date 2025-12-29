namespace LibrarySystem.Common.Messaging
{
    public class UserDeactivatedMessage : RabbitMqMessage
    {
        public int UserId { get; set; }     
        public DateTime OccurredAt { get; set; }
    }
}
