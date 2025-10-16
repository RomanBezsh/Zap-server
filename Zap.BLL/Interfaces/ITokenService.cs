using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zap.BLL.DTO;

namespace Zap.BLL.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(UserDTO user);
    }
}
