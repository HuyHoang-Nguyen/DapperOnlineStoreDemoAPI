namespace Demo.Domain.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId, string email, IList<string>? roles);
    }
}
