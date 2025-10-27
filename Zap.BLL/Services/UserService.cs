using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zap.BLL.DTO;
using Zap.BLL.Interfaces;
using Zap.DAL.Entities;
using Zap.DAL.Interfaces;
using Zap.DAL.Repositories;

namespace Zap.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow ?? throw new ArgumentNullException(nameof(uow));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task CreateUserAsync(UserDTO userDTO)
        {
            if (userDTO == null) throw new ArgumentNullException(nameof(userDTO));

            var user = new User
            {
                Username = userDTO.Username ?? "Unknown",
                Email = userDTO.Email ?? "",
                PasswordHash = userDTO.PasswordHash ?? "",
                DisplayName = string.IsNullOrWhiteSpace(userDTO.DisplayName) ? userDTO.Username : userDTO.DisplayName,
                DateOfBirth = userDTO.DateOfBirth == default ? DateTime.UtcNow : userDTO.DateOfBirth,
                PhoneNumber = userDTO.PhoneNumber ?? "",
                ProfileImageUrl = userDTO.ProfileImageUrl ?? "",
                Bio = userDTO.Bio ?? "",
                CreatedAt = DateTime.UtcNow,
                IsEmailVerified = false,
                IsSuspended = false
            };

            await _db.Users.AddAsync(user);
            await _db.SaveAsync();
        }

        public async Task UpdateUserAsync(UserDTO userDTO)
        {
            if (userDTO == null)
                throw new ArgumentNullException(nameof(userDTO));

            var user = await _db.Users.GetByIdAsync(userDTO.Id);
            if (user == null)
                throw new KeyNotFoundException($"User with id {userDTO.Id} not found.");

            // Основные данные
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

            // ⚠️ Эти поля мы игнорируем, т.к. их нет в базе
            // user.VerificationCode = userDTO.VerificationCode;
            // user.CodeExpiration = userDTO.CodeExpiration;

            _db.Users.Update(user);
            await _db.SaveAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _db.Users.GetByIdAsync(id);
            if (user != null)
            {
                _db.Users.Delete(user);
                await _db.SaveAsync();
            }
        }

        public async Task<UserDTO?> GetUserAsync(int id)
        {
            var user = await _db.Users.GetByIdAsync(id);
            if (user == null) return null;

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO?> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            if (string.IsNullOrWhiteSpace(usernameOrEmail)) return null;

            var users = await _db.Users.GetAllAsync();
            var normalized = usernameOrEmail.Trim().ToLower();

            var user = users.FirstOrDefault(u =>
                (u.Username != null && u.Username.Trim().ToLower() == normalized) ||
                (u.Email != null && u.Email.Trim().ToLower() == normalized));


            if (user == null) return null;

            return _mapper.Map<UserDTO>(user);
        }
        public async Task<IEnumerable<UserDTO>> SearchUsersByUsernameAsync(string partialUsername)
        {
            if (string.IsNullOrWhiteSpace(partialUsername))
                return new List<UserDTO>();

            var users = await _db.Users.GetAllAsync();

            var matchedUsers = users
                .Where(u => u.Username.Contains(partialUsername, StringComparison.OrdinalIgnoreCase))
                .Select(u => _mapper.Map<UserDTO>(u))
                .ToList();

            return matchedUsers;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _db.Users.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task FollowUserAsync(int userId, int targetUserId)
        {
            if (userId == targetUserId)
                throw new InvalidOperationException("Нельзя подписаться на самого себя.");

            var user = await _db.Users.GetByIdAsync(userId);
            var target = await _db.Users.GetByIdAsync(targetUserId);
            if (user == null || target == null)
                throw new KeyNotFoundException("User(s) not found.");

            var follows = await _db.UserFollows.GetAllAsync();
            var alreadyFollowing = follows.Any(uf => uf.FollowerId == userId && uf.FollowedId == targetUserId);
            if (alreadyFollowing)
                return;

            var follow = new UserFollow
            {
                FollowerId = userId,
                FollowedId = targetUserId,
                CreatedAt = DateTime.UtcNow
            };

            await _db.UserFollows.AddAsync(follow);
            await _db.SaveAsync();
        }

        public async Task UnfollowUserAsync(int userId, int targetUserId)
        {
            var follows = await _db.UserFollows.GetAllAsync();
            var follow = follows.FirstOrDefault(uf => uf.FollowerId == userId && uf.FollowedId == targetUserId);
            if (follow != null)
            {
                _db.UserFollows.Delete(follow);
                await _db.SaveAsync();
            }
        }

        public async Task<bool> IsFollowingAsync(int userId, int targetUserId)
        {
            var follows = await _db.UserFollows.GetAllAsync();
            return follows.Any(f => f.FollowerId == userId && f.FollowedId == targetUserId);
        }

        public async Task<IEnumerable<UserShortDTO>> GetFollowersAsync(int userId)
        {
            var follows = await _db.UserFollows.GetAllAsync();

            var followerIds = follows
                .Where(f => f.FollowedId == userId)
                .Select(f => f.FollowerId)
                .ToList();

            if (!followerIds.Any())
                return new List<UserShortDTO>();

            var users = await _db.Users.GetAllAsync();
            var followers = users
                .Where(u => followerIds.Contains(u.Id))
                .Select(u => new UserShortDTO
                {
                    Id = u.Id,
                    Username = u.Username,
                    ProfileImageUrl = u.ProfileImageUrl
                })
                .ToList();

            return followers;
        }
		public async Task<IEnumerable<UserShortDTO>> GetFollowingAsync(int userId)
		{
			var follows = await _db.UserFollows.GetAllAsync();

			var followingIds = follows
				.Where(f => f.FollowerId == userId)
				.Select(f => f.FollowedId)
				.ToList();

			if (!followingIds.Any())
				return new List<UserShortDTO>();

			var users = await _db.Users.GetAllAsync();
			var following = users
				.Where(u => followingIds.Contains(u.Id))
				.Select(u => new UserShortDTO
				{
					Id = u.Id,
					Username = u.Username,
					ProfileImageUrl = u.ProfileImageUrl
				})
				.ToList();

			return following;
		}

	}
}
