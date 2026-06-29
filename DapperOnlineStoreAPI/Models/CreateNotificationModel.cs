namespace DapperOnlineStoreAPI.Models
{
    public class CreateNotificationModel
    {
        public Guid? UserId {  get; set; }
        public string? Message { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
