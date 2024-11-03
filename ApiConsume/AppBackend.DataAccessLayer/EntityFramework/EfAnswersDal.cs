using AppBackend.DataAccessLayer.Abstract;
using AppBackend.DataAccessLayer.Common;
using AppBackend.DataAccessLayer.Concrete;
using AppBackend.DataAccessLayer.Repositories;
using AppBackend.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DataAccessLayer.EntityFramework
{
    public class EfAnswersDal : GenericRepository<Answers>, IAnswersDal
    {
        private readonly Context _context;
        public EfAnswersDal(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<OperationResult<IEnumerable<Answers>>> DeleteAnswersByQuestionIdAsync(int questionId)
        {
            try
            {
                var answersToDelete = await _context.Set<Answers>()
                                                     .Where(a => a.QuestionId == questionId)
                                                     .ToListAsync();
                _context.Set<Answers>().RemoveRange(answersToDelete);
                await _context.SaveChangesAsync();

                return OperationResult<IEnumerable<Answers>>.Success(answersToDelete, "Silme işlemi başarılı.", "CRUD-DELETE-SUCCESS");
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<Answers>>.Failure($"Silme işlemi başarısız: {ex.Message}", "CRUD-DELETE-ERROR-EXCEPTION");
            }
        }

    }
}
