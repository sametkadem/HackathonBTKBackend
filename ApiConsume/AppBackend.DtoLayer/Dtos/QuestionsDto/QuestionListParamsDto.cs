using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DtoLayer.Dtos.QuestionsDto
{
    public class QuestionListParamsDto
    {
        public int CategoryId { get; set; } = 0;
        public int SubCategoryId { get; set; } = 0;
        public string? Question { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10000;
        public string OrderBy { get; set; } = "Id";
        public string OrderDirection { get; set; } = "desc";
        public bool OnlyLastAnswer { get; set; } = false;
    }
}
