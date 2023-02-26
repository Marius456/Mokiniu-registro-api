using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Mokiniu_registro_api.DTOs;
using Mokiniu_registro_api.Models;
using Mokiniu_registro_api.Services.Interfaces;
using Mokiniu_registro_api.Services.Responses;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mokiniu_registro_api.Services
{
    public class ParentService : IParentService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _config;

        public ParentService(AppDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }
        public async Task<LoginResponse<AuthenticatedUserDTO>> Login(LoginDTO UserCredentials)
        {
            var user = await _dbContext.Parents.SingleOrDefaultAsync(u => u.Email == UserCredentials.Email);
            if (user == null)
            {
                var errorMessage = $"Password or login is incorrect";
                Log.Error(errorMessage);
                return new LoginResponse<AuthenticatedUserDTO> { Message = errorMessage, Success = false };
            }

            if (!UserCredentials.Password.Equals(user.Password))
            {
                var errorMessage = $"Password or login is incorrect";
                Log.Error(errorMessage);
                return new LoginResponse<AuthenticatedUserDTO> { Message = errorMessage, Success = false };
            }

            var token = GenerateJwtToken(user);
            var authenticatedUserDTO = new AuthenticatedUserDTO()
            {
                Email = user.Email,
                Token = token
            };
            return new LoginResponse<AuthenticatedUserDTO> { Data = authenticatedUserDTO };
        }

        public async Task<IEnumerable<Parent>> GetAll()
        {
            return await _dbContext.Parents.ToListAsync();
        }

        public async Task<ParentResponse> GetById(int id)
        {
            var parent = await _dbContext.Parents.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (parent == null)
            {
                string errorMessage = "Parent not found.";

                Log.Error(errorMessage);

                return new ParentResponse(errorMessage);
            }

            return new ParentResponse(parent);
        }

        public async Task<ParentResponse> Create(Parent parent)
        {
            var existingUser = await _dbContext.Parents.SingleOrDefaultAsync(u => u.Email == parent.Email);
            if (existingUser != null)
            {
                string errorMessage = $"User with Email: {existingUser.Email} already exists";
                return new ParentResponse(errorMessage);
            }

            try
            {
                await _dbContext.Parents.AddAsync(parent);
                await _dbContext.SaveChangesAsync();
                return new ParentResponse(parent);
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when creating the item: {exception.Message}";
                return new ParentResponse(errorMessage);
            }
        }

        public async Task<ParentResponse> Update(int id, Parent updatedParent, string userMail)
        {
            var parent = await _dbContext.Parents.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (parent == null)
            {
                throw new KeyNotFoundException();
            }

            var user = await _dbContext.Parents.SingleOrDefaultAsync(u => u.Email == userMail);
            if (parent.Id != user.Id)
            {
                string errorMessage = "You have no permition to edit this account information.";
                Log.Error(errorMessage);
                return new ParentResponse(errorMessage, false);
            }

            parent.Id = updatedParent.Id;
            parent.Name = updatedParent.Name;
            parent.Email = updatedParent.Email;
            parent.Password = updatedParent.Password;

            if (parent.Name == "")
            {
                string errorMessage = "Parent name not found.";
                Log.Error(errorMessage);
                return new ParentResponse(errorMessage, true);
            }
            if (parent.Email == "")
            {
                string errorMessage = "Parent email not found.";
                Log.Error(errorMessage);
                return new ParentResponse(errorMessage, true);
            }
            if (parent.Password == "")
            {
                string errorMessage = "Parent password not found.";
                Log.Error(errorMessage);
                return new ParentResponse(errorMessage, true);
            }
            try
            {
                _dbContext.Parents.Update(parent);
                await _dbContext.SaveChangesAsync();
                return new ParentResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when updating the item: {exception.Message}";
                return new ParentResponse(errorMessage, true);
            }
        }

        public async Task<ParentResponse> Delete(int id, string userMail)
        {
            var parent = await _dbContext.Parents.FirstOrDefaultAsync(x => x.Id.Equals(id));
            if (parent == null)
            {
                throw new KeyNotFoundException();
            }

            var user = await _dbContext.Parents.SingleOrDefaultAsync(u => u.Email == userMail);
            if (parent.Id != user.Id)
            {
                string errorMessage = "You have no permition to delete this parent's account.";
                Log.Error(errorMessage);
                return new ParentResponse(errorMessage, false);
            }

            try
            {
                _dbContext.Parents.Remove(parent);
                await _dbContext.SaveChangesAsync();
                return new ParentResponse();
            }
            catch (Exception exception)
            {
                string errorMessage = $"An error occured when deleting the item: {exception.Message}";
                return new ParentResponse(errorMessage, true);
            }
        }
        private string GenerateJwtToken(Parent user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(ClaimTypes.Name, user.Email.ToString()));

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                permClaims,
                expires: DateTime.Now.AddMinutes(500),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
