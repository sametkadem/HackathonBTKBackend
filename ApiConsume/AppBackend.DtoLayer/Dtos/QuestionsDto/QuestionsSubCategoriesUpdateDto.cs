using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DtoLayer.Dtos.QuestionsDto
{
    public class QuestionsSubCategoriesUpdateDto
    {
        [Required(ErrorMessage = "Sub Katagori id boş bırakılamaz")]
        public int Id { get; set; }
     
        [Required(ErrorMessage = "Katagori adı boş bırakılamaz")]
        public required string Name { get; set; }
    }
}
