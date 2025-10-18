using FluentAssertions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zap.BLL.Interfaces
{
    public interface IEmailService
    {
        Task SendVerificationCodeAsync(string email, string code);
    }
}
