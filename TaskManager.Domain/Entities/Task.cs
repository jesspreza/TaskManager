using TaskManager.Domain.Validations;

namespace TaskManager.Domain.Entities
{
    public partial class Task
    {
        public Task(string name, string? description, Guid projectId) : base()
        {
            NameValidator.ValidateName(name);
            ValidateProjectId(projectId);
            Name = name;
            Description = description;
        }

        public void UpdateDomain(string name, string? description)
        {
            NameValidator.ValidateName(name);
            Name = name;
            Description = description;
            Update();
        }

        public static void ValidateProjectId(Guid projectId)
        {
            DomainExceptionValidation.When(projectId == Guid.Empty, "Project cannot be empty");
        }
    }
}
