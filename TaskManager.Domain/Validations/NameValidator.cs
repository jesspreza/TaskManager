namespace TaskManager.Domain.Validations
{
    public static class NameValidator
    {
        public static void ValidateName(string name)
        {
            DomainExceptionValidation.When(string.IsNullOrWhiteSpace(name), "Name cannot be null or empty");
            DomainExceptionValidation.When(name.Length > 250, "Name cannot be longer than 250 characters");
        }
    }
}
