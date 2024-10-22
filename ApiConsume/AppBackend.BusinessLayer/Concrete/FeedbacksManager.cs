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
    public class FeedbacksManager : IFeedbacksService
    {
        private readonly IFeedbacksDal _feedbacksDal;

        public FeedbacksManager(IFeedbacksDal feedbacksDal)
        {
            _feedbacksDal = feedbacksDal;
        }
        public Task<OperationResult<int>> TCount(Expression<Func<Feedbacks, bool>>? predicate = null)
        {
            return _feedbacksDal.Count(predicate);
        }

        public Task<OperationResult<Feedbacks>> TDelete(int id)
        {
            return _feedbacksDal.Delete(id);
        }

        public Task<OperationResult<bool>> TExists(Expression<Func<Feedbacks, bool>> predicate)
        {
            return _feedbacksDal.Exists(predicate);
        }

        public Task<OperationResult<IEnumerable<Feedbacks>>> TFind(Expression<Func<Feedbacks, bool>> predicate)
        {
            return _feedbacksDal.Find(predicate);
        }

        public Task<OperationResult<IEnumerable<Feedbacks>>> TGetAll()
        {
            return _feedbacksDal.GetAll();
        }

        public Task<OperationResult<IEnumerable<Feedbacks>>> TGetByCondition(Expression<Func<Feedbacks, bool>> predicate)
        {
            return _feedbacksDal.GetByCondition(predicate);
        }

        public Task<OperationResult<Feedbacks>> TGetById(int id)
        {
            return _feedbacksDal.GetById(id);
        }

        public Task<OperationResult<PagedResult<Feedbacks>>> TGetPaged(int pageNumber, int pageSize, Expression<Func<Feedbacks, bool>>? predicate = null)
        {
            return _feedbacksDal.GetPaged(pageNumber, pageSize, predicate);
        }

        public Task<OperationResult<Feedbacks>> TInsert(Feedbacks entity)
        {
            return _feedbacksDal.Insert(entity);
        }

        public Task<OperationResult<Feedbacks>> TUpdate(Feedbacks entity)
        {
            return _feedbacksDal.Update(entity);
        }
    }
}
