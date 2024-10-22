using AppBackend.EntityLayer.Concrete.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.EntityLayer.Concrete
{
    public class Answers
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public required Questions Question { get; set; }
        public int UserId { get; set; }
        public required AppUser AppUser { get; set; }
        public string? Answer { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
