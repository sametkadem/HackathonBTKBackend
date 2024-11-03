using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DtoLayer.Dtos.QuestionsDto
{
    public class QuestionsSubCategoryAddDto
    {
        [Required(ErrorMessage = "Kategori adı boş geçilemez")]
        public required string SubCategoryName { get; set; }
        public int ParentId { get; set; } = 0;
        public int CategoryId { get; set; }
    }
}
