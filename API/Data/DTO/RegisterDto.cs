using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.DTO
{
    public class RegisterDto : LoginDto
    {
        public string Email { get; set; }
    }
}