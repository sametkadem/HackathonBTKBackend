using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.EntityLayer.Concrete
{
    public class QuestionsCategories
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string DisplayName { get; set; }
        public string Path { get; set; }
        public int? ParentId { get; set; }
        public QuestionsCategories? Parent { get; set; }
        public bool IsLeaf { get; set; }
        public bool IsRoot { get; set; }        
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<QuestionsCategories> SubCategories { get; set; } // Alt kategoriler için ilişki

        public ICollection<Questions> Questions { get; set; } // Sorular için ilişki

    }
}
