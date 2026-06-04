namespace Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string username, IEnumerable<string> roles);
        string GenerateRefreshToken();
    }
}
