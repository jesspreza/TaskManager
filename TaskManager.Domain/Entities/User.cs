namespace TaskManager.Domain.Entities
{
    public partial class User
    {
        public User(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public void SetPassword(string password)
        {
            Password = password;
        }
    }
}
