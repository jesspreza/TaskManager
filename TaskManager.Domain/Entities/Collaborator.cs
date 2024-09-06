using TaskManager.Domain.Validations;

namespace TaskManager.Domain.Entities
{
    public partial class Collaborator
    {
        public Collaborator(string name, Guid userId) : base()
        {
            NameValidator.ValidateName(name);
            Name = name;
            UserId = userId;
        }

        public void UpdateDomain(string name)
        {
            NameValidator.ValidateName(name);
            Name = name;
            Update();
        }
    }
}
