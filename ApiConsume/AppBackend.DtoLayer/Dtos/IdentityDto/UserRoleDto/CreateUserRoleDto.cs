using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DtoLayer.Dtos.IdentityDto.UserRoleDto
{
    public class CreateUserRoleDto
    {
        [Required(ErrorMessage = "Kullanıcı adı alanı boş geçilemez")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Rol adı alanı boş geçilemez")]
        public string Role { get; set; }
    }
}
