namespace FleetManagement.Application.Interfaces.Validations
{
    public interface IValidationResolver<T> where T : class
    {
        void Resolve(T entity);
    }
}