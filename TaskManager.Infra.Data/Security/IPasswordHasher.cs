namespace TaskManager.Infra.Data.Security
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
    }
}