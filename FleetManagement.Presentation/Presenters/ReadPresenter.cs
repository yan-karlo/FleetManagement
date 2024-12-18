using FleetManagement.Presentation.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.Presentation.Presenters
{
    public class ReadPresenter<T>
    {
        private readonly Func<Task<T>> _serviceMethod;

        public ReadPresenter(Func<Task<T>> serviceMethod)
        {
            _serviceMethod = serviceMethod;
        }

        public async Task<IActionResult> Execute()
        {
            try
            {
                var result = await _serviceMethod();
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return ErrorResultFactory.handle(e);
            }
        }
    }
}
