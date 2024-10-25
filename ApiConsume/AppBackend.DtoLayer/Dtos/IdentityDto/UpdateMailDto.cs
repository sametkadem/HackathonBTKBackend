using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DtoLayer.Dtos.IdentityDto
{
    public class UpdateMailDto
    {
        public required string CurrentPassword { get; set; }
        public required string NewEmail { get; set; }
    }
}
