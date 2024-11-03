using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DtoLayer.Dtos.QuestionsDto
{
    public class QuestionsAddDto
    {
        public int CategoryId { get; set; } = 1;
        public int SubCategoryId { get; set; } = 1;
        public string? Question { get; set; }
        public IFormFile? QuestionImage { get; set; }
    }
}
