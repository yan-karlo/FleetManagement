namespace FleetManagement.Presentation.DTOs
{
    public class ErrorDTO
    {
        public string ErrorMessage { get; set; }
        public string InnerErrorMessage { get; set; }

        public ErrorDTO(Exception e)
        {
            ErrorMessage = e.Message;
            InnerErrorMessage = e.InnerException?.Message ?? e.Message;
        }
        public ErrorDTO(string message, Exception e)
        {
            ErrorMessage = message;
            InnerErrorMessage = e.InnerException?.Message ?? e.InnerException?.Message ?? e.Message;
        }
    }
}
