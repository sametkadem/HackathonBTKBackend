using AppBackend.DataAccessLayer.Abstract;
using AppBackend.DataAccessLayer.Common;
using AppBackend.DataAccessLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        private readonly Context _context;

        public GenericRepository(Context context)
        {

            _context = context;
        }

        public async Task<OperationResult<int>> Count(Expression<Func<T, bool>>? predicate = null)
        {
            try
            {
                IQueryable<T> query = predicate == null ? _context.Set<T>() : _context.Set<T>().Where(predicate);
                var count = await query.CountAsync(); 

                return OperationResult<int>.Success(count, "Kayıt sayısı başarıyla alındı.", "CRUD-COUNT-SUCCESS");
            }
            catch (Exception ex)
            {
                return OperationResult<int>.Failure($"Kayıt sayısı alınırken bir hata oluştu: {ex.Message}", "CRUD-COUNT-ERROR-EXCEPTION");
            }
        }

        public async Task<OperationResult<T>> Delete(int id)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity == null)
                {
                    return OperationResult<T>.Failure("Kayıt bulunamadı.", "CRUD-DELETE-ERROR-ENTITY-NOT-FOUND");
                }
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
                return OperationResult<T>.Success(entity, "Silme işlemi başarılı.", "CRUD-DELETE-SUCCESS");
            }
            catch (Exception ex)
            {
                return OperationResult<T>.Failure($"Silme işlemi başarısız: {ex.Message}", "CRUD-DELETE-ERROR-EXCEPTION");
            }
        }

        public async Task<OperationResult<bool>> Exists(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var exists = await _context.Set<T>().AnyAsync(predicate);
                return OperationResult<bool>.Success(exists, exists ? "Kayıt bulundu." : "Kayıt bulunamadı.", "CRUD-EXISTSBYCONDITION-SUCCESS");
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"Kayıt olup olmadığı aranırken bir hata oluştu: {ex.Message}", "CRUD-EXISTSBYCONDITION-ERROR-EXCEPTION");
            }
        }

        public async Task<OperationResult<IEnumerable<T>>> Find(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var entities = await _context.Set<T>().Where(predicate).ToListAsync();
                return OperationResult<IEnumerable<T>>.Success(entities, "Kayıtlar bulundu.", "CRUD-FIND-SUCCESS");
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<T>>.Failure($"Kayıtlar bulunurken bir hata oluştu: {ex.Message}", "CRUD-FIND-ERROR-EXCEPTION");
            }
        }

        public async Task<OperationResult<IEnumerable<T>>> GetAll()
        {
            try
            {
                var entities = await _context.Set<T>().ToListAsync();
                return OperationResult<IEnumerable<T>>.Success(entities, "Tüm kayıtlar getirildi.", "CRUD-GETALL-SUCCESS");
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<T>>.Failure($"Tüm kayıtlar getirilirken bir hata oluştu: {ex.Message}", "CRUD-GETALL-ERROR-EXCEPTION");
            }
        }

        public async Task<OperationResult<IEnumerable<T>>> GetByCondition(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var entities = await _context.Set<T>().Where(predicate).ToListAsync();
                return OperationResult<IEnumerable<T>>.Success(entities, "Kayıtlar bulundu.", "CRUD-GETBYCONDITION-SUCCESS");
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<T>>.Failure($"Kayıtlar getirilirken bir hata oluştu: {ex.Message}", "CRUD-GETBYCONDITION-ERROR-EXCEPTION");
            }
        }

        public async Task<OperationResult<T>> GetById(int id)
        {
            try
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity == null)
                {
                    return OperationResult<T>.Failure("Kayıt bulunamadı.", "CRUD-GETBYID-ERROR-ENTITY-NOT-FOUND");
                }

                return OperationResult<T>.Success(entity, "Kayıt bulundu.", "CRUD-GETBYID-SUCCESS");
            }
            catch (Exception ex)
            {
                return OperationResult<T>.Failure($"Kayıt getirilirken bir hata oluştu: {ex.Message}", "CRUD-GETBYID-ERROR-EXCEPTION");
            }
        }

        public async Task<OperationResult<PagedResult<T>>> GetPaged(int pageNumber, int pageSize, Expression<Func<T, bool>>? predicate = null)
        {
            try
            {
                IQueryable<T> query = predicate == null ? _context.Set<T>() : _context.Set<T>().Where(predicate);
                var totalRecords = await query.CountAsync(); 
                var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                var hasNextPage = pageNumber < totalPages;

                var entities = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var pagedResult = new PagedResult<T>
                {
                    Data = entities,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = totalRecords,
                    TotalPages = totalPages,
                    HasNextPage = hasNextPage
                };

                return OperationResult<PagedResult<T>>.Success(pagedResult, "Kayıtlar sayfalı olarak getirildi.", "CRUD-GETPAGED-SUCCESS");
            }
            catch (Exception ex)
            {
                return OperationResult<PagedResult<T>>.Failure($"Kayıtlar sayfalı olarak getirilirken bir hata oluştu: {ex.Message}", "CRUD-GETPAGED-ERROR-EXCEPTION");
            }
        }

        public async Task<OperationResult<T>> Insert(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();
                return OperationResult<T>.Success(entity, "Kayıt eklendi.", "CRUD-INSERT-SUCCESS");
            }
            catch (Exception ex)
            {
                return OperationResult<T>.Failure($"Kayıt eklenirken bir hata oluştu: {ex.Message}", "CRUD-INSERT-ERROR-EXCEPTION");
            }
        }

        public async Task<OperationResult<T>> Update(T entity)
        {
            try
            {
                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();
                return OperationResult<T>.Success(entity, "Kayıt güncellendi.", "CRUD-UPDATE-SUCCESS");
            }
            catch (Exception ex)
            {
                return OperationResult<T>.Failure($"Kayıt güncellenirken bir hata oluştu: {ex.Message}", "CRUD-UPDATE-ERROR-EXCEPTION");
            }
        }
    }
}
