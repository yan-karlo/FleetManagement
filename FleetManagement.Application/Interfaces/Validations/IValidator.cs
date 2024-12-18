namespace FleetManagement.Application.Interfaces.Validations
{
    public interface IValidator<T> where T : class
    {
        void Validate(T entity);
    }
}
