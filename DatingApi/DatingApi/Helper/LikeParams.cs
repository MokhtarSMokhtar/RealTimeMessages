namespace DatingApi.Helper
{
    public class LikeParams:PaginationParam
    {
        public int UserId { get; set; }
        public string Predicate { get; set; }

    }
}
