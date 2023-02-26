using Microsoft.EntityFrameworkCore;
using Mokiniu_registro_api.Models;
using Mokiniu_registro_api.Services.Interfaces;
using Mokiniu_registro_api.Services.Responses;
using Serilog;

namespace Mokiniu_registro_api.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly AppDbContext _dbContext;

        public SchoolService(AppDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        public async Task<IEnumerable<string>> GetChildrenSchoolsNames(string parentEmail)
        {
            var user = await _dbContext.Parents.FirstOrDefaultAsync(parent => parent.Email == parentEmail);

            if (user == null)
            {
                return null;
            }

            var children = await _dbContext.Children.Where(r => r.ParentId.Equals(user.Id)).ToListAsync();

            HashSet<string> schoolNames = new HashSet<string>();

            foreach (var child in children)
            {
                schoolNames.Add((await _dbContext.Schools.FindAsync(child.SchoolId)).Name);
            }
            return schoolNames;
        }

        public async Task<IEnumerable<School>> GetAll()
        {
            return await _dbContext.Schools.ToListAsync();
        }

        public async Task<SchoolResponse> GetById(int id)
        {
            var school = await _dbContext.Schools.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (school == null)
            {
                string errorMessage = "School not found.";

                Log.Error(errorMessage);

                return new SchoolResponse(errorMessage);
            }

            return new SchoolResponse(school);
        }

        public async Task<SchoolResponse> Create(School school)
        {

            if (school.Name == null)
            {
                string errorMessage = "School name not found.";
                Log.Error(errorMessage);
                return new SchoolResponse(errorMessage);
            }

            try
            {
                await _dbContext.Schools.AddAsync(school);
                await _dbContext.SaveChangesAsync();
                return new SchoolResponse(school);
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when creating the item: {exception.Message}";
                return new SchoolResponse(errorMessage);
            }
        }

        public async Task<SchoolResponse> Update(int id, School updatedSchool)
        {
            var school = await _dbContext.Schools.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (school == null)
            {
                throw new KeyNotFoundException();
            }

            school.Id = updatedSchool.Id;
            school.Name = updatedSchool.Name;

            if (school.Name == null)
            {
                string errorMessage = "School name not found.";
                Log.Error(errorMessage);
                return new SchoolResponse(errorMessage);
            }
            try
            {
                _dbContext.Schools.Update(school);
                await _dbContext.SaveChangesAsync();
                return new SchoolResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new SchoolResponse(errorMessage);
            }
        }
        public async Task<SchoolResponse> Delete(int id)
        {
            var school = await _dbContext.Schools.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (school == null)
            {
                throw new KeyNotFoundException();
            }

            try
            {
                _dbContext.Schools.Remove(school);
                await _dbContext.SaveChangesAsync();
                return new SchoolResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when deleting the item: {exception.Message}";
                return new SchoolResponse(errorMessage);
            }
        }
    }
}
