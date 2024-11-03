using AppBackend.DataAccessLayer.Common;
using AppBackend.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DataAccessLayer.Abstract
{
    public interface IAnswersDal : IGenericDal<Answers>
    {
        Task<OperationResult<IEnumerable<Answers>>> DeleteAnswersByQuestionIdAsync(int questionId);
    }
}
