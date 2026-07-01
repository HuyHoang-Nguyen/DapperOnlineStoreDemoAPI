namespace Demo.Domain.Models
{
    public class UserLoginModel
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}
