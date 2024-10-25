using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DtoLayer.Dtos.IdentityDto
{
    public class UpdateUserInfoDto
    {
        [Required (ErrorMessage = "Kullanıcı adı boş bırakılamaz")]
        public string? FirstName { get; set; }
        [Required (ErrorMessage = "Soyadı boş bırakılamaz")]
        public string? LastName { get; set; }
        [Required (ErrorMessage = "Telefon numarası boş bırakılamaz")]
        public string? PhoneNumber { get; set; }

    }
}
