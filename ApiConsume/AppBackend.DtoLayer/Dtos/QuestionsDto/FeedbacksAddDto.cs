using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DtoLayer.Dtos.QuestionsDto
{
    public class FeedbacksAddDto
    {
        [Required(ErrorMessage = "Feedback boş geçilemez")]
        public string Feedback { get; set; }
        public int? QuestionId { get; set; }
    }
}
