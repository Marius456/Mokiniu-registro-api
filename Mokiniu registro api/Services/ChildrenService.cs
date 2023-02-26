using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mokiniu_registro_api.Models;
using Mokiniu_registro_api.Services.Interfaces;
using Mokiniu_registro_api.Services.Responses;
using Serilog;

namespace Mokiniu_registro_api.Services
{
    public class ChildrenService : IChildrenService
    {
        private readonly AppDbContext _dbContext;

        public ChildrenService(AppDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task<IEnumerable<Child>> GetAll()
        {
            return await _dbContext.Children.ToListAsync();
        }

        public async Task<ChildrenResponse> GetById(int id)
        {
            var child = await _dbContext.Children.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (child == null)
            {
                string errorMessage = "Child not found.";

                Log.Error(errorMessage);

                return new ChildrenResponse(errorMessage);
            }

            return new ChildrenResponse(child);
        }

        public async Task<ChildrenResponse> Create(Child child)
        {

            if (child.Name == null)
            {
                string errorMessage = "Child name not found.";
                Log.Error(errorMessage);
                return new ChildrenResponse(errorMessage);
            }
            if (child.ParentId == 0)
            {
                string errorMessage = "Child parent ID not found.";
                Log.Error(errorMessage);
                return new ChildrenResponse(errorMessage);
            }
            if (child.SchoolId == 0)
            {
                string errorMessage = "Child school ID not found.";
                Log.Error(errorMessage);
                return new ChildrenResponse(errorMessage);
            }

            try
            {
                await _dbContext.Children.AddAsync(child);
                await _dbContext.SaveChangesAsync();
                return new ChildrenResponse(child);
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when creating the item: {exception.Message}";
                return new ChildrenResponse(errorMessage);
            }
        }

        public async Task<ChildrenResponse> Update(int id, Child updatedChild, string userMail)
        {
            var child = await _dbContext.Children.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (child == null)
            {
                throw new KeyNotFoundException();
            }

            var user = await _dbContext.Parents.SingleOrDefaultAsync(u => u.Email == userMail);
            if (child.ParentId != user.Id)
            {
                string errorMessage = "You have no permition to edit this child information.";
                Log.Error(errorMessage);
                return new ChildrenResponse(errorMessage, false);
            }

            child.Id = updatedChild.Id;
            child.Name = updatedChild.Name;
            child.ParentId = updatedChild.ParentId;
            child.SchoolId = updatedChild.SchoolId;

            if (child.Name == null)
            {
                string errorMessage = "Child name not found.";
                Log.Error(errorMessage);
                return new ChildrenResponse(errorMessage, true);
            }
            try
            {
                _dbContext.Children.Update(child);
                await _dbContext.SaveChangesAsync();
                return new ChildrenResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new ChildrenResponse(errorMessage, true);
            }
        }
        public async Task<ChildrenResponse> Delete(int id, string userMail)
        {
            var child = await _dbContext.Children.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (child == null)
            {
                throw new KeyNotFoundException();
            }

            var user = await _dbContext.Parents.SingleOrDefaultAsync(u => u.Email == userMail);
            if (child.ParentId != user.Id)
            {
                string errorMessage = "You have no permition to delete this child information.";
                Log.Error(errorMessage);
                return new ChildrenResponse(errorMessage, false);
            }

            try
            {
                _dbContext.Children.Remove(child);
                await _dbContext.SaveChangesAsync();
                return new ChildrenResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when deleting the item: {exception.Message}";
                return new ChildrenResponse(errorMessage, true);
            }
        }
    }
}
