using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DtoLayer.Dtos.IdentityDto
{
    public class UpdatePasswordDto
    {
        [Required(ErrorMessage = "Mevcut şifre alanı boş geçilemez")]
        public required string CurrentPassword { get; set; }
        [Required(ErrorMessage = "Yeni şifre alanı boş geçilemez")]
        public required string NewPassword { get; set; }
        [Required(ErrorMessage = "Yeni şifre tekrarı alanı boş geçilemez")]
        public required string ConfirmPassword { get; set; }
    }
}
