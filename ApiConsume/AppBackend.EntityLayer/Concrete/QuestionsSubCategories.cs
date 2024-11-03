using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.EntityLayer.Concrete
{
    public class QuestionsSubCategories
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public required string Name { get; set; }
        public required string DisplayName { get; set; }
        public string? Path { get; set; }
        public int? ParentId { get; set; }
        public QuestionsSubCategories? Parent { get; set; } // Self-referencing parent subcategory
        public ICollection<QuestionsSubCategories> SubCategories { get; set; } = new List<QuestionsSubCategories>(); // Child subcategories
        public bool IsLeaf { get; set; }
        public bool IsRoot { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required QuestionsCategories Category { get; set; }
        public ICollection<Questions> Questions { get; set; } = new List<Questions>();
    }
}
