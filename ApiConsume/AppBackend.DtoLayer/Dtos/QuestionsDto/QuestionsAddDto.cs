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
        public int? CategoryId { get; set; }
        public string? Question { get; set; }
        public IFormFile? QuestionImage { get; set; }
    }
}
