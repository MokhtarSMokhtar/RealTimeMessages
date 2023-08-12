namespace DatingApi.Helper
{
    public class MessageParams:PaginationParam
    {
        public string Username { get; set; }
        public string Container { get; set; } = "Unread";
    }
}
