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
            ProjectId = projectId;
        }

        public void UpdateDomain(string name, string? description, Guid projectId)
        {
            NameValidator.ValidateName(name);
            Name = name;
            Description = description;
            ProjectId = projectId;
            Update();
        }

        public static void ValidateProjectId(Guid projectId)
        {
            DomainExceptionValidation.When(projectId == Guid.Empty, "A Project must be associated with the Task");
        }
    }
}
