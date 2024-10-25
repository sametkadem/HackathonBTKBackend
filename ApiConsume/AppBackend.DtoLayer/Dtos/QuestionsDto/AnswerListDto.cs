using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DtoLayer.Dtos.QuestionsDto
{
    public class AnswerListDto
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public DateTime CreatedAt { get; set;}
        public DateTime UpdatedAt { get; set;}

    }
}
