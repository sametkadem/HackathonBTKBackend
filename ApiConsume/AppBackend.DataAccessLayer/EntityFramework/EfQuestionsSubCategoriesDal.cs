using AppBackend.DataAccessLayer.Abstract;
using AppBackend.DataAccessLayer.Concrete;
using AppBackend.DataAccessLayer.Repositories;
using AppBackend.DtoLayer.Dtos.QuestionsDto;
using AppBackend.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DataAccessLayer.EntityFramework
{
    public class EfQuestionsSubCategoriesDal : GenericRepository<QuestionsSubCategories>, IQuestionsSubCategoriesDal
    {
        private readonly Context _context;

        public EfQuestionsSubCategoriesDal(Context context) : base(context)
        {
            _context = context;
        }

        // Hiyerarşik tüm kategorileri çekmek için metot
        public List<QuestionsSubCategoryListDto> GetAllSubCategoriesWithHierarchy(int CategoryId)
        {
            // Önce tüm alt kategorileri alıyoruz
            var allSubCategories = _context.Set<QuestionsSubCategories>()
                .Where(sc => sc.IsActive == true && sc.CategoryId == CategoryId)
                .ToList();

            // Ardından recursive fonksiyon ile tüm hiyerarşiyi oluşturuyoruz
            return GetAllLeafSubCategories(null, allSubCategories);
        }

        // Recursive fonksiyon: ParentId'ye göre alt kategorileri bulur ve iç içe ekler
        private List<QuestionsSubCategoryListDto> GetAllLeafSubCategories(int? parentId, List<QuestionsSubCategories> allSubCategories)
        {
            // ParentId ile eşleşen tüm alt kategorileri buluyoruz
            var subCategories = allSubCategories
                .Where(sc => sc.ParentId == parentId)
                .ToList();

            // Her bir alt kategori için tekrar alt kategorilerini buluyoruz
            var subCategoryDtos = new List<QuestionsSubCategoryListDto>();
            QuestionsCategories generalCategory = null;
            foreach (var subCategory in subCategories)
            {
                if (generalCategory == null)
                {
                    generalCategory = _context.Set<QuestionsCategories>().Where(c => c.Id == subCategory.CategoryId).FirstOrDefault();
                }
                var dto = new QuestionsSubCategoryListDto
                {
                    Id = subCategory.Id,
                    GeneralId = (generalCategory != null ? generalCategory.Id : 0),
                    GeneralName = (generalCategory != null ? generalCategory.Name : ""),
                    GeneralPath = (generalCategory != null ? generalCategory.Path : ""),
                    Name = subCategory.Name,
                    ParentId = subCategory.ParentId,
                    Path = subCategory.Path,
                    IsLeaf = subCategory.IsLeaf, // Alt kategori olup olmadığını kontrol ediyoruz
                    IsRoot = subCategory.IsRoot, // Kök kategori olup olmadığını kontrol ediyoruz
                    SubCategories = GetAllLeafSubCategories(subCategory.Id, allSubCategories) // Alt kategorileri dökümana ekle
                };

                subCategoryDtos.Add(dto);
            }

            return subCategoryDtos;
        }
    }
}
