using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DtoLayer.Dtos.IdentityDto.RegisterDto
{
    public class CreateNewUserDto
    {
        [Required(ErrorMessage = "Ad alanı boş geçilemez")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad alanı boş geçilemez")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Telefon numarası alanı boş geçilemez")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email alanı boş geçilemez")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı boş geçilemez")]
        [StringLength(100, ErrorMessage = "Şifre en az {6} karakter olmalıdır", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı alanı boş geçilemez")]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmPassword { get; set; }
    }
}
