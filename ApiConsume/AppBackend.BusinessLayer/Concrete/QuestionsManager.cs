using AppBackend.BusinessLayer.Abstract;
using AppBackend.DataAccessLayer.Abstract;
using AppBackend.DataAccessLayer.Common;
using AppBackend.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.BusinessLayer.Concrete
{
    public class QuestionsManager : IQuestionsService
    {
        private readonly IQuestionsDal _questionsDal;
        public QuestionsManager(IQuestionsDal questionsDal)
        {
            _questionsDal = questionsDal;
        }
        public Task<OperationResult<int>> TCount(Expression<Func<Questions, bool>>? predicate = null)
        {
            return _questionsDal.Count(predicate);
        }

        public Task<OperationResult<Questions>> TDelete(int id)
        {
            return _questionsDal.Delete(id);
        }

        public Task<OperationResult<bool>> TExists(Expression<Func<Questions, bool>> predicate)
        {
            return _questionsDal.Exists(predicate);
        }

        public Task<OperationResult<IEnumerable<Questions>>> TFind(Expression<Func<Questions, bool>> predicate)
        {
            return _questionsDal.Find(predicate);
        }

        public Task<OperationResult<IEnumerable<Questions>>> TGetAll()
        {
            return _questionsDal.GetAll();
        }

        public Task<OperationResult<IEnumerable<Questions>>> TGetByCondition(Expression<Func<Questions, bool>> predicate)
        {
            return _questionsDal.GetByCondition(predicate);
        }

        public Task<OperationResult<Questions>> TGetById(int id)
        {
            return _questionsDal.GetById(id);
        }

        public Task<OperationResult<PagedResult<Questions>>> TGetPaged(int pageNumber, int pageSize, Expression<Func<Questions, bool>>? predicate = null)
        {
            return _questionsDal.GetPaged(pageNumber, pageSize, predicate);
        }

        public Task<OperationResult<Questions>> TInsert(Questions entity)
        {
            return _questionsDal.Insert(entity);
        }

        public Task<OperationResult<Questions>> TUpdate(Questions entity)
        {
            return _questionsDal.Update(entity);
        }
    }
}
