using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zap.BLL.DTO
{
    public record LoginRequestDTO(string UsernameOrEmail, string Password);
}
