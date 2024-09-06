using TaskManager.Domain.Validations;

namespace TaskManager.Domain.Entities
{
    public partial class Project
    {
        public Project(string name) : base()
        {
            NameValidator.ValidateName(name);
            Name = name;
        }

        public void UpdateDomain(string name)
        {
            NameValidator.ValidateName(name);
            Name = name;
            Update();
        }
    }
}
