using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Zap.BLL.DTO;
using Zap.BLL.Interfaces;
using Zap.DAL.Entities;
using Zap.DAL.Interfaces;

namespace Zap.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _database;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork uow, IMapper mapper)
        {
            _database = uow ?? throw new ArgumentNullException(nameof(uow));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task CreateUser(UserDTO userDTO)
        {
            if (userDTO == null) throw new ArgumentNullException(nameof(userDTO));

            var user = new User
            {
                Username = userDTO.Username,
                Email = userDTO.Email,
                PasswordHash = userDTO.PasswordHash,
                DisplayName = userDTO.DisplayName,
                DateOfBirth = userDTO.DateOfBirth,
                PhoneNumber = userDTO.PhoneNumber,
                ProfileImageUrl = userDTO.ProfileImageUrl,
                Bio = userDTO.Bio,
                CreatedAt = userDTO.CreatedAt == default ? DateTime.UtcNow : userDTO.CreatedAt,
                IsEmailVerified = userDTO.IsEmailVerified,
                IsSuspended = userDTO.IsSuspended
            };

            await _database.Users.AddAsync(user);
            await _database.SaveAsync();
        }

        public async Task UpdateUser(UserDTO userDTO)
        {
            if (userDTO == null) throw new ArgumentNullException(nameof(userDTO));

            var user = await _database.Users.GetByIdAsync(userDTO.Id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with id {userDTO.Id} not found.");
            }

            user.Username = userDTO.Username;
            user.Email = userDTO.Email;
            user.PasswordHash = userDTO.PasswordHash;
            user.DisplayName = userDTO.DisplayName;
            user.DateOfBirth = userDTO.DateOfBirth;
            user.PhoneNumber = userDTO.PhoneNumber;
            user.ProfileImageUrl = userDTO.ProfileImageUrl;
            user.Bio = userDTO.Bio;
            user.IsEmailVerified = userDTO.IsEmailVerified;
            user.IsSuspended = userDTO.IsSuspended;

            _database.Users.Update(user);
            await _database.SaveAsync();
        }

        public async Task DeleteUser(int id)
        {
            var user = await _database.Users.GetByIdAsync(id);
            if (user != null)
            {
                _database.Users.Delete(user);
                await _database.SaveAsync();
            }
        }

        public async Task<UserDTO?> GetUser(int id)
        {
            var user = await _database.Users.GetByIdAsync(id);
            if (user == null) return null;

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO?> GetUserByUsernameOrEmail(string usernameOrEmail)
        {
            if (string.IsNullOrWhiteSpace(usernameOrEmail)) return null;

            var users = await _database.Users.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);

            if (user == null) return null;

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            var users = await _database.Users.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }
    }
}
