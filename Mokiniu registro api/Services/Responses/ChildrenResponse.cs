using Mokiniu_registro_api.Models;

namespace Mokiniu_registro_api.Services.Responses
{
    public class ChildrenResponse : BaseResponse
    {
        public Child Child { get; set; }

        public ChildrenResponse(Child item) : base(string.Empty, true)
        {
            this.Child = item;
        }

        public ChildrenResponse() : base(string.Empty, true, true)
        {
        }

        public ChildrenResponse(string message) : base(message, false)
        {
        }

        public ChildrenResponse(string message, bool authorise) : base(message, false, authorise)
        {
        }
    }
}
