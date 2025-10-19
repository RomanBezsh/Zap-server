using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zap.BLL.DTO
{
    public class UserShortDTO
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
