using AppBackend.EntityLayer.Concrete.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.EntityLayer.Concrete
{
    public class Questions
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required AppUser AppUser { get; set; }
        public int? CategoryId { get; set; }
        public QuestionsCategories Category { get; set; }
        public string? QuestionFilePath { get; set; }
        public string? QuestionFileName { get; set; }
        public string? QuestionFileType { get; set; }
        public string? QuestionFileUrl { get; set; }
        public string? Question { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
