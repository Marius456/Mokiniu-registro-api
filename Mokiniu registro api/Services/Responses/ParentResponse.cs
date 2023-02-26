using Mokiniu_registro_api.Models;

namespace Mokiniu_registro_api.Services.Responses
{
    public class ParentResponse : BaseResponse
    {
        public Parent Parent { get; set; }

        public ParentResponse(Parent item) : base(string.Empty, true)
        {
            this.Parent = item;
        }

        public ParentResponse() : base(string.Empty, true, true)
        {
        }

        public ParentResponse(string message) : base(message, false)
        {
        }

        public ParentResponse(string message, bool authorise) : base(message, false, authorise)
        {
        }
    }
}
