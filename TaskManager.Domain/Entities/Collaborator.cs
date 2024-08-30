using TaskManager.Domain.Validations;

namespace TaskManager.Domain.Entities
{
    public partial class Collaborator
    {
        public Collaborator(string name) : base()
        {
            NameValidator.ValidateName(name);
            Name = name;
        }

        public void UpdateName(string name)
        {
            NameValidator.ValidateName(name);
            Name = name;
            Update();
        }
    }
}
