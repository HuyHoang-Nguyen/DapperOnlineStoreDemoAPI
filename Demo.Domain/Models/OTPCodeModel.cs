namespace Demo.Domain.Models
{
    public class OTPCodeModel
    {
        public string? Email { get; set; }
        public string? Code { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
