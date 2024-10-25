using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DtoLayer.Dtos.QuestionsDto
{
    public class QuestionsCategoriesAddDto
    {
        [Required (ErrorMessage = "Kategori adı boş geçilemez")]
        public required string CategoryName { get; set; }       
        public int ParentId { get; set; } = 0;
    }
}
