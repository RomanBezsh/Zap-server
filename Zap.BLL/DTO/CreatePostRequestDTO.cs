using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Zap.BLL.DTO
{
    public class CreatePostRequestDTO
    {
        public string Content { get; set; } = string.Empty;
        public IFormFile? File { get; set; }
    }
}
