namespace DapperOnlineStoreAPI.Models.QueryModel
{
    public class GetUserQueryModel
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

    }
}
