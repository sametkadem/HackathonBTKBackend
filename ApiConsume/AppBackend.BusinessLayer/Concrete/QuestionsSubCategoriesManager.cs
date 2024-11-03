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
    public class QuestionsSubCategoriesManager : IQuestionsSubCategoriesService
    {
        private readonly IQuestionsSubCategoriesDal _questionsSubCategoriesDal;

        public QuestionsSubCategoriesManager(IQuestionsSubCategoriesDal questionsSubCategoriesDal)
        {
            _questionsSubCategoriesDal = questionsSubCategoriesDal;
        }
        public Task<OperationResult<int>> TCount(Expression<Func<QuestionsSubCategories, bool>>? predicate = null)
        {
            return _questionsSubCategoriesDal.Count(predicate);
        }

        public Task<OperationResult<QuestionsSubCategories>> TDelete(int id)
        {
            return _questionsSubCategoriesDal.Delete(id);
        }

        public Task<OperationResult<bool>> TExists(Expression<Func<QuestionsSubCategories, bool>> predicate)
        {
            return _questionsSubCategoriesDal.Exists(predicate);
        }

        public Task<OperationResult<IEnumerable<QuestionsSubCategories>>> TFind(Expression<Func<QuestionsSubCategories, bool>> predicate)
        {
            return _questionsSubCategoriesDal.Find(predicate);
        }

        public Task<OperationResult<IEnumerable<QuestionsSubCategories>>> TGetAll()
        {
            return _questionsSubCategoriesDal.GetAll();
        }

        public List<QuestionsSubCategoryListDto> TGetAllSubCategoriesWithHierarchy(int CategoryId)
        {
            return _questionsSubCategoriesDal.GetAllSubCategoriesWithHierarchy(CategoryId);
        }

        public Task<OperationResult<IEnumerable<QuestionsSubCategories>>> TGetByCondition(Expression<Func<QuestionsSubCategories, bool>> predicate)
        {
            return _questionsSubCategoriesDal.GetByCondition(predicate);
        }

        public Task<OperationResult<QuestionsSubCategories>> TGetById(int id)
        {
            return _questionsSubCategoriesDal.GetById(id);
        }

        public Task<OperationResult<PagedResult<QuestionsSubCategories>>> TGetPaged(int pageNumber, int pageSize, Expression<Func<QuestionsSubCategories, bool>>? predicate = null)
        {
            return _questionsSubCategoriesDal.GetPaged(pageNumber, pageSize, predicate);
        }

        public Task<OperationResult<QuestionsSubCategories>> TInsert(QuestionsSubCategories entity)
        {
            return _questionsSubCategoriesDal.Insert(entity);
        }

        public Task<OperationResult<QuestionsSubCategories>> TUpdate(QuestionsSubCategories entity)
        {
            return _questionsSubCategoriesDal.Update(entity);
        }
    }
}
