using AppBackend.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DtoLayer.Dtos.QuestionsDto
{
    public class QuestionsListDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string QuestionImage { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryPath { get; set; }
        public string CategoryParentName { get; set; }
        public int? CategoryParentId { get; set; }
        public List<AnswerListDto> Answers { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
