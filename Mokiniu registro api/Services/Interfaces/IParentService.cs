using Mokiniu_registro_api.DTOs;
using Mokiniu_registro_api.Models;
using Mokiniu_registro_api.Services.Responses;

namespace Mokiniu_registro_api.Services.Interfaces
{
    public interface IParentService
    {
        Task<IEnumerable<Parent>> GetAll();
        Task<ParentResponse> GetById(int id);
        Task<ParentResponse> Create(Parent parent);
        Task<ParentResponse> Update(int id, Parent updatedParent, string userMail);
        Task<ParentResponse> Delete(int id, string userMail);
        Task<LoginResponse<AuthenticatedUserDTO>> Login(LoginDTO UserCredentials);
    }
}
