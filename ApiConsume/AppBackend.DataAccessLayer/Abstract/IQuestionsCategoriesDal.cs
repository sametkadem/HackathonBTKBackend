using AppBackend.DtoLayer.Dtos.QuestionsDto;
using AppBackend.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DataAccessLayer.Abstract
{
    public interface IQuestionsCategoriesDal : IGenericDal<QuestionsCategories>
    {
        public List<QuestionsCategoryListDto> GetAllSubCategoriesWithHierarchy();

    }
}
