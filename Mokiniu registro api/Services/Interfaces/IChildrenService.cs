using Microsoft.AspNetCore.Mvc;
using Mokiniu_registro_api.Models;
using Mokiniu_registro_api.Services.Responses;

namespace Mokiniu_registro_api.Services.Interfaces
{
    public interface IChildrenService
    {
        Task<IEnumerable<Child>> GetAll();
        Task<ChildrenResponse> GetById(int id);
        Task<ChildrenResponse> Create(Child child);
        Task<ChildrenResponse> Update(int id, Child updatedChild, string userMail);
        Task<ChildrenResponse> Delete(int id, string userMail);
    }
}
