using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zap.BLL.DTO
{
    public class UserDTO
    {
        public class User
        {
            public int Id { get; set; }

            // Уникальное имя пользователя (@handle)
            public string Username { get; set; } = string.Empty;

            // Email для входа и подтверждения
            public string Email { get; set; } = string.Empty;

            // Хешированный пароль
            public string PasswordHash { get; set; } = string.Empty;

            // Отображаемое имя (может быть любым)
            public string DisplayName { get; set; } = string.Empty;

            // Дата рождения
            public DateTime DateOfBirth { get; set; }

            // Фото профиля (ссылка на файл)
            public string ProfileImageUrl { get; set; } = string.Empty;

            // Биография пользователя
            public string Bio { get; set; } = string.Empty;

            // Дата регистрации
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            // Подтверждён ли email
            public bool IsEmailVerified { get; set; } = false;

            // Статус блокировки аккаунта
            public bool IsSuspended { get; set; } = false;
        }
    }
}
