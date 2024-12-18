using FleetManagement.Presentation.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.Presentation.Presenters
{
    public class RemovePresenter
    {
        private readonly Func<Task> _serviceMethod;

        public RemovePresenter(Func<Task> serviceMethod)
        {
            _serviceMethod = serviceMethod;
        }

        public async Task<IActionResult> Execute()
        {
            try
            {
                await _serviceMethod();
                return new NoContentResult();
            }
            catch (Exception e)
            {
                return ErrorResultFactory.handle(e);
            }
        }
    }
}
