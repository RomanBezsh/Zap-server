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
        Task CreateUser(UserDTO userDTO);
        Task UpdateUser(UserDTO userDTO);
        Task DeleteUser(int id);
        Task<UserDTO?> GetUser(int id);
        Task<IEnumerable<UserDTO>> SearchUsersByUsername(string partialUsername);

        Task<UserDTO> GetUserByUsernameOrEmail(string usernameOrEmail);
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task FollowUser(int userId, int targetUserId);
        Task UnfollowUser(int userId, int targetUserId);
        Task<bool> IsFollowingAsync(int userId, int targetUserId);
        Task<IEnumerable<UserShortDTO>> GetFollowersAsync(int userId);
        Task<IEnumerable<UserShortDTO>> GetFollowingAsync(int userId);
    }
}
