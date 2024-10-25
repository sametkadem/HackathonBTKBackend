using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DtoLayer.Dtos.QuestionsDto
{
    public class QuestionsUpdateDto
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string? Question { get; set; }
    }
}
