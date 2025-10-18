using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zap.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<string> AuthenticateAndGenerateTokenAsync(string username, string password);
        Task<string> GenerateVerificationCodeAsync(string email); 
        Task<bool> VerifyCodeAsync(string email, string code);
    }
}
