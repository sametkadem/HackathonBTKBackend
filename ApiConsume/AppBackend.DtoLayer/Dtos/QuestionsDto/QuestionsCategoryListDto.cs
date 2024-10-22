using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DtoLayer.Dtos.QuestionsDto
{
    public class QuestionsCategoryListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int? ParentId { get; set; }
        public bool IsLeaf { get; set; }
        public bool IsRoot { get; set; }
        public List<QuestionsCategoryListDto> SubCategories { get; set; } = new List<QuestionsCategoryListDto>();
    }
}
