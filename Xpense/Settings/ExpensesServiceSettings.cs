namespace Xpense.Settings
{
    internal record ExpensesServiceSettings
    {
        public Credentials Credentials { get; set; }
        public string Url { get; set; }
    }

    internal record Credentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
