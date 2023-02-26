using Mokiniu_registro_api.DTOs.Errors;

namespace Mokiniu_registro_api.DTOs
{
    public class ErrorDTO
    {
        public List<Error> Errors { get; set; }

        public ErrorDTO()
        {
            Errors = new List<Error>();
        }
    }
}
