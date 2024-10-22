using AppBackend.DataAccessLayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DataAccessLayer.Abstract
{
    public interface IGenericDal<T> where T : class
    {
        Task<OperationResult<T>> Insert (T entity); // Ekle
        Task<OperationResult<T>> GetById (int id); // ID ile getir
        Task<OperationResult<IEnumerable<T>>> GetAll(); // Tüm verileri getir
        Task<OperationResult<T>> Update(T entity); // Güncelle
        Task<OperationResult<T>> Delete(int id); // Sil
        Task<OperationResult<IEnumerable<T>>> Find(Expression<Func<T, bool>> predicate); // Koşula göre bul
        Task<OperationResult<PagedResult<T>>> GetPaged(int pageNumber, int pageSize, Expression<Func<T, bool>>? predicate = null); // Koşula göre sayfalandırma
        Task<OperationResult<int>> Count(Expression<Func<T, bool>>? predicate = null); // Koşula göre say
        Task<OperationResult<bool>> Exists(Expression<Func<T, bool>> predicate); // Koşula göre var mı
        Task<OperationResult<IEnumerable<T>>> GetByCondition(Expression<Func<T, bool>> predicate); // Koşula göre getir
    }
}
