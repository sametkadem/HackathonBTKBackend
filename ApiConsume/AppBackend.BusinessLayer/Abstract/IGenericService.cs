using AppBackend.DataAccessLayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.BusinessLayer.Abstract
{
    public interface IGenericService<T> where T : class
    {
        Task<OperationResult<T>> TInsert(T entity); // Ekle
        Task<OperationResult<T>> TGetById(int id); // ID ile getir
        Task<OperationResult<IEnumerable<T>>> TGetAll(); // Tüm verileri getir
        Task<OperationResult<T>> TUpdate(T entity); // Güncelle
        Task<OperationResult<T>> TDelete(int id); // Sil
        Task<OperationResult<IEnumerable<T>>> TFind(Expression<Func<T, bool>> predicate); // Koşula göre bul
        Task<OperationResult<PagedResult<T>>> TGetPaged(int pageNumber, int pageSize, Expression<Func<T, bool>>? predicate = null); // Koşula göre sayfalandırma
        Task<OperationResult<int>> TCount(Expression<Func<T, bool>>? predicate = null); // Koşula göre say
        Task<OperationResult<bool>> TExists(Expression<Func<T, bool>> predicate); // Koşula göre var mı
        Task<OperationResult<IEnumerable<T>>> TGetByCondition(Expression<Func<T, bool>> predicate); // Koşula göre getir
    }
}
