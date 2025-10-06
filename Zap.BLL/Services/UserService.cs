using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zap.DAL.Interfaces;
using Zap.DAL.Entities;
using Zap.BLL.DTO;
using Zap.BLL.Interfaces;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Zap.BLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task CreateUser(UserDTO userDTO)
        {
            User user = new User
            {
                Username = userDTO.Username,
                Email = userDTO.Email,
                PasswordHash = userDTO.PasswordHash,
                DisplayName = userDTO.DisplayName,
                DateOfBirth = userDTO.DateOfBirth,
                ProfileImageUrl = userDTO.ProfileImageUrl,
                Bio = userDTO.Bio,
                CreatedAt = userDTO.CreatedAt,
                IsEmailVerified = userDTO.IsEmailVerified,
                IsSuspended = userDTO.IsSuspended
            };
            await Database.Users.AddAsync(user);
        }
        public async Task UpdateUser(UserDTO userDTO)
        {
            var user = await Database.Users.GetByIdAsync(userDTO.Id);
            if (user != null)
            {
                user.Username = userDTO.Username;
                user.Email = userDTO.Email;
                user.PasswordHash = userDTO.PasswordHash;
                user.DisplayName = userDTO.DisplayName;
                user.DateOfBirth = userDTO.DateOfBirth;
                user.ProfileImageUrl = userDTO.ProfileImageUrl;
                user.Bio = userDTO.Bio;
                user.IsEmailVerified = userDTO.IsEmailVerified;
                user.IsSuspended = userDTO.IsSuspended;
                Database.Users.Update(user);
            }
        }
        public async Task DeleteUser(int id)
        {
            var user = await Database.Users.GetByIdAsync(id);
            if (user != null)
            {
                Database.Users.Delete(user);
            }
        }
        public async Task<UserDTO?> GetUser(int id)
        {
            var user = await Database.Users.GetByIdAsync(id);
            if (user != null)
            {
                return new UserDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    DisplayName = user.DisplayName,
                    DateOfBirth = user.DateOfBirth,
                    ProfileImageUrl = user.ProfileImageUrl,
                    Bio = user.Bio,
                    CreatedAt = user.CreatedAt,
                    IsEmailVerified = user.IsEmailVerified,
                    IsSuspended = user.IsSuspended
                };
            }
            return null;
        }
        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            var users = await Database.Users.GetAllAsync();
            return users.Select(user => new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                DisplayName = user.DisplayName,
                DateOfBirth = user.DateOfBirth,
                ProfileImageUrl = user.ProfileImageUrl,
                Bio = user.Bio,
                CreatedAt = user.CreatedAt,
                IsEmailVerified = user.IsEmailVerified,
                IsSuspended = user.IsSuspended
            });
        }

    }
}
