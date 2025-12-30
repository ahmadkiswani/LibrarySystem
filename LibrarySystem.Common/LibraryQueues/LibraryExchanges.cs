namespace LibrarySystem.Common.Messaging
{
    public static class LibraryExchanges
    {
        public const string Users = "library.users.exchange";
    }

    public static class LibraryRoutingKeys
    {
        public const string UserCreated = "users.created";
        public const string UserUpdated = "users.updated";
        public const string UserDeactivated = "users.deactivated";
    }
}
