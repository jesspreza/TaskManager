namespace TaskManager.Domain.Account
{
    public interface IAuthenticate
    {
        string GenerateToken(Guid userId, string username);
        bool ValidatePassword(string enteredPassword, string storedHash);
        string CryptoPassword(string password);
    }
}
