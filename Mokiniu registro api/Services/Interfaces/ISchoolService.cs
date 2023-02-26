using Microsoft.AspNetCore.Mvc;
using Mokiniu_registro_api.Models;
using Mokiniu_registro_api.Services.Responses;

namespace Mokiniu_registro_api.Services.Interfaces
{
    public interface ISchoolService
    {
        Task<IEnumerable<School>> GetAll();
        Task<IEnumerable<string>> GetChildrenSchoolsNames(string parentEmail);
        Task<SchoolResponse> GetById(int id);
        Task<SchoolResponse> Create(School school);
        Task<SchoolResponse> Update(int id, School updatedSchool);
        Task<SchoolResponse> Delete(int id);
    }
}
