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
    public class AnswersManager : IAnswersService
    {
        private readonly IAnswersDal _answersDal;

        public AnswersManager(IAnswersDal answersDal)
        {
            _answersDal = answersDal;
        }
        public Task<OperationResult<int>> TCount(Expression<Func<Answers, bool>>? predicate = null)
        {
            return _answersDal.Count(predicate);
        }

        public Task<OperationResult<Answers>> TDelete(int id)
        {
            return _answersDal.Delete(id);
        }

        public Task<OperationResult<bool>> TExists(Expression<Func<Answers, bool>> predicate)
        {
            return _answersDal.Exists(predicate);
        }

        public Task<OperationResult<IEnumerable<Answers>>> TFind(Expression<Func<Answers, bool>> predicate)
        {
            return _answersDal.Find(predicate);
        }

        public Task<OperationResult<IEnumerable<Answers>>> TGetAll()
        {
            return _answersDal.GetAll();
        }

        public Task<OperationResult<IEnumerable<Answers>>> TGetByCondition(Expression<Func<Answers, bool>> predicate)
        {
            return _answersDal.GetByCondition(predicate);
        }

        public Task<OperationResult<Answers>> TGetById(int id)
        {
            return _answersDal.GetById(id);
        }

        public Task<OperationResult<PagedResult<Answers>>> TGetPaged(int pageNumber, int pageSize, Expression<Func<Answers, bool>>? predicate = null)
        {
            return _answersDal.GetPaged(pageNumber, pageSize, predicate);
        }

        public Task<OperationResult<Answers>> TInsert(Answers entity)
        {
            return _answersDal.Insert(entity);
        }

        public Task<OperationResult<Answers>> TUpdate(Answers entity)
        {
            return _answersDal.Update(entity);
        }
    }
}
