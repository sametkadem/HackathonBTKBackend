using AppBackend.DtoLayer.Dtos.QuestionsDto;
using AppBackend.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.BusinessLayer.Abstract
{
    public interface IQuestionsCategoriesService : IGenericService<QuestionsCategories>
    {
        public List<QuestionsCategoryListDto> TGetAllSubCategoriesWithHierarchy();
    }
}
