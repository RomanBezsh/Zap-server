using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zap.BLL.DTO;
using Zap.DAL.Interfaces;

namespace Zap.BLL.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(UserDTO userDTO);
        Task UpdateUserAsync(UserDTO userDTO);
        Task DeleteUserAsync(int id);
        Task<UserDTO?> GetUserAsync(int id);
        Task<IEnumerable<UserDTO>> SearchUsersByUsernameAsync(string partialUsername);
        Task<UserDTO?> GetUserByUsernameOrEmailAsync(string usernameOrEmail);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task FollowUserAsync(int userId, int targetUserId);
        Task UnfollowUserAsync(int userId, int targetUserId);
        Task<bool> IsFollowingAsync(int userId, int targetUserId);
        Task<IEnumerable<UserShortDTO>> GetFollowersAsync(int userId);
        Task<IEnumerable<UserShortDTO>> GetFollowingAsync(int userId);
    }
}
