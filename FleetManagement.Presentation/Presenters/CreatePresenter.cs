using FleetManagement.Presentation.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.Presentation.Presenters
{
    public class CreatePresenter<T>
    {
        private readonly Func<Task<T>> _serviceMethod;

        public CreatePresenter(Func<Task<T>> serviceMethod)
        {
            _serviceMethod = serviceMethod;
        }

        public async Task<IActionResult> Execute()
        {
            try
            {
                var result = await _serviceMethod();
                return new CreatedResult(string.Empty, result);
            }
            catch (Exception e)
            {
                return ErrorResultFactory.handle(e);
            }
        }
    }
}
