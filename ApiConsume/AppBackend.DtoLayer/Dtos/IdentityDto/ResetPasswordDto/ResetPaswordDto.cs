using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DtoLayer.Dtos.IdentityDto.ResetPasswordDto
{
    public class ResetPaswordDto
    {
        [Required(ErrorMessage = "Kullanıcı id alanı boş geçilemez")]
        public string userId { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş geçilemez")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı alanı boş geçilemez")]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Token alanı boş geçilemez.")]
        public string token { get; set; }

    }
}
