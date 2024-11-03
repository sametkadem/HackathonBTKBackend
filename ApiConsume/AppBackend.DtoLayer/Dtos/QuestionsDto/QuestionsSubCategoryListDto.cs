using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DtoLayer.Dtos.QuestionsDto
{
    public class QuestionsSubCategoryListDto
    {
        public int Id { get; set; }
        public int GeneralId { get; set; }
        public string GeneralName { get; set; }
        public string GeneralPath { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int? ParentId { get; set; }
        public bool IsLeaf { get; set; }
        public bool IsRoot { get; set; }
        public List<QuestionsSubCategoryListDto> SubCategories { get; set; } = new List<QuestionsSubCategoryListDto>();
    }
}
