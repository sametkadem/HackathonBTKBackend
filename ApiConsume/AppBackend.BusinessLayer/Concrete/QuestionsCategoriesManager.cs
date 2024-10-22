using AppBackend.BusinessLayer.Abstract;
using AppBackend.DataAccessLayer.Abstract;
using AppBackend.DataAccessLayer.Common;
using AppBackend.DtoLayer.Dtos.QuestionsDto;
using AppBackend.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.BusinessLayer.Concrete
{
    public class QuestionsCategoriesManager : IQuestionsCategoriesService
    {
        private readonly IQuestionsCategoriesDal _QuestionsCategoriesDal;

        public QuestionsCategoriesManager(IQuestionsCategoriesDal QuestionsCategoriesDal)
        {
            _QuestionsCategoriesDal = QuestionsCategoriesDal;
        }

        public Task<OperationResult<int>> TCount(Expression<Func<QuestionsCategories, bool>>? predicate = null)
        {
            return _QuestionsCategoriesDal.Count(predicate);
        }

        public Task<OperationResult<QuestionsCategories>> TDelete(int id)
        {
            return _QuestionsCategoriesDal.Delete(id);
        }

        public Task<OperationResult<bool>> TExists(Expression<Func<QuestionsCategories, bool>> predicate)
        {
            return _QuestionsCategoriesDal.Exists(predicate);
        }

        public Task<OperationResult<IEnumerable<QuestionsCategories>>> TFind(Expression<Func<QuestionsCategories, bool>> predicate)
        {
            return _QuestionsCategoriesDal.Find(predicate);
        }

        public Task<OperationResult<IEnumerable<QuestionsCategories>>> TGetAll()
        {
            return _QuestionsCategoriesDal.GetAll();
        }

        public List<QuestionsCategoryListDto> TGetAllSubCategoriesWithHierarchy()
        {
            return _QuestionsCategoriesDal.GetAllSubCategoriesWithHierarchy();
        }


        public Task<OperationResult<IEnumerable<QuestionsCategories>>> TGetByCondition(Expression<Func<QuestionsCategories, bool>> predicate)
        {
            return _QuestionsCategoriesDal.GetByCondition(predicate);
        }

        public Task<OperationResult<QuestionsCategories>> TGetById(int id)
        {
            return _QuestionsCategoriesDal.GetById(id);
        }

        public Task<OperationResult<PagedResult<QuestionsCategories>>> TGetPaged(int pageNumber, int pageSize, Expression<Func<QuestionsCategories, bool>>? predicate = null)
        {
            return _QuestionsCategoriesDal.GetPaged(pageNumber, pageSize, predicate);
        }

        public Task<OperationResult<QuestionsCategories>> TInsert(QuestionsCategories entity)
        {
            return _QuestionsCategoriesDal.Insert(entity);
        }

        public Task<OperationResult<QuestionsCategories>> TUpdate(QuestionsCategories entity)
        {
            return _QuestionsCategoriesDal.Update(entity);
        }
    }
}
