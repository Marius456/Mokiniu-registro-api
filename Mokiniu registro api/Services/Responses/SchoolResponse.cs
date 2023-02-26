using Mokiniu_registro_api.Models;

namespace Mokiniu_registro_api.Services.Responses
{
    public class SchoolResponse : BaseResponse
    {
        public School School { get; set; }

        public SchoolResponse(School item) : base(string.Empty, true)
        {
            this.School = item;
        }

        public SchoolResponse() : base(string.Empty, true, true)
        {
        }

        public SchoolResponse(string message) : base(message, false)
        {
        }
    }
}
