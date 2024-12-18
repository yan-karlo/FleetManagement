using FleetManagement.Presentation.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Presentation.Utilities
{
    public static class ErrorResultFactory
    {
        public static IActionResult handle(Exception e)
        {
            return e switch
            {
                ArgumentNullException
                or ArgumentException
                => new BadRequestObjectResult(new ErrorDTO(e)),

                DbUpdateException => new ConflictObjectResult(new ErrorDTO(e)),

                SqlException => new ConflictObjectResult(new ErrorDTO(e)),

                KeyNotFoundException => new NotFoundObjectResult(new ErrorDTO(e)),

                UnauthorizedAccessException => new UnauthorizedObjectResult(new ErrorDTO(e)),

                TimeoutException => new ObjectResult(new ErrorDTO(e)) { StatusCode = StatusCodes.Status408RequestTimeout },

                _ => new ObjectResult(new ErrorDTO(e)) { StatusCode = StatusCodes.Status500InternalServerError },
            };
        }
    }
}
