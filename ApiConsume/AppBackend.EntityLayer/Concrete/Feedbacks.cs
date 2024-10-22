using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.EntityLayer.Concrete
{
    public class Feedbacks
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? QuestionId { get; set; }
        public Questions? Question { get; set; }
        public string? Feedback { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
