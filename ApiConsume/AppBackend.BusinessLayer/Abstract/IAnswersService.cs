using AppBackend.DataAccessLayer.Common;
using AppBackend.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.BusinessLayer.Abstract
{
    public interface IAnswersService : IGenericService<Answers>
    {
        Task<OperationResult<IEnumerable<Answers>>> TDeleteAnswersByQuestionIdAsync(int questionId);
    }
}
