using AppBackend.BusinessLayer.Abstract;
using AppBackend.DtoLayer.Dtos.QuestionsDto;
using AppBackend.EntityLayer.Concrete;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppBackend.WebApi.Controllers.QuestionsController
{
    [Route("api/v1/questions/sub-category/")]
    [ApiController]
    public class QuestionsSubCategoryController : ControllerBase
    {

        private readonly IQuestionsSubCategoriesService _questionsSubCategoriesService;
        private readonly IQuestionsCategoriesService _questionsCategoriesService;
        private readonly IMapper _mapper;

        public QuestionsSubCategoryController(IQuestionsSubCategoriesService questionsSubCategoriesService, IQuestionsCategoriesService questionsCategoriesService, IMapper mapper)
        {
            _questionsSubCategoriesService = questionsSubCategoriesService;
            _questionsCategoriesService = questionsCategoriesService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] QuestionsSubCategoryAddDto questionsSubCategoryAddDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Status = 0,
                    Message = "Girdi verilerinde hata var",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }
            try
            {
                var questionsSubCategory = _mapper.Map<QuestionsSubCategories>(questionsSubCategoryAddDto);
                var isRoot = (questionsSubCategoryAddDto.ParentId == 0) ? true : false;
                var path = "";
                if (!isRoot)
                {
                    var parent = await _questionsSubCategoriesService.TGetById(questionsSubCategoryAddDto.ParentId);
                    if (parent.Status != "success" || !parent.IsSuccess || parent.Data == null)
                    {
                        return BadRequest(new { Status = 0, Message = "Üst kategori bulunamadı" });
                    }
                    parent.Data.IsLeaf = false;
                    parent.Data.IsRoot = true;
                    parent.Data.UpdatedAt = DateTime.Now;
                    var resultparent = await _questionsSubCategoriesService.TUpdate(parent.Data);
                    if (resultparent.Status != "success" || !resultparent.IsSuccess)
                    {
                        return BadRequest(new { Status = 0, Message = resultparent.Messages });
                    }
                    path = parent.Data.Path;
                }
                var questionsCategory = await _questionsCategoriesService.TGetById(questionsSubCategoryAddDto.CategoryId);
                if (questionsCategory.Status != "success" || !questionsCategory.IsSuccess || questionsCategory.Data == null)
                {
                    return BadRequest(new { Status = 0, Message = "Ana kategori bulunamadı" });
                }
                path += (path == "") ? questionsSubCategoryAddDto.SubCategoryName : " -> " + questionsSubCategoryAddDto.SubCategoryName;
                questionsSubCategory.Name = questionsSubCategoryAddDto.SubCategoryName;
                questionsSubCategory.DisplayName = questionsSubCategoryAddDto.SubCategoryName;
                questionsSubCategory.IsActive = true;
                questionsSubCategory.ParentId = (isRoot) ? null : questionsSubCategoryAddDto.ParentId;
                questionsSubCategory.IsRoot = isRoot;
                questionsSubCategory.IsLeaf = true;
                questionsSubCategory.Path = path;
                questionsSubCategory.CategoryId = questionsCategory.Data.Id;
                questionsSubCategory.Category = questionsCategory.Data;
                questionsSubCategory.CreatedAt = DateTime.Now;
                questionsSubCategory.UpdatedAt = DateTime.Now;
                var result = await _questionsSubCategoriesService.TInsert(questionsSubCategory);
                if (result.Status == "success" && result.IsSuccess)
                {
                    return Ok(new { Status = 1, Message = "Kategori başarıyla eklendi" });
                }
                return BadRequest(new { Status = 0, Message = result.Messages, Data = result.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = 0, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] QuestionsSubCategoriesUpdateDto questionsSubCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Status = 0,
                    Message = "Girdi verilerinde hata var",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }
            try
            {
                var existingCategory = await _questionsSubCategoriesService.TGetById(questionsSubCategoryDto.Id);
                if (existingCategory.Status != "success" || !existingCategory.IsSuccess || existingCategory.Data == null)
                {
                    return BadRequest(new { Status = 0, Message = "Kategori bulunamadı" });
                }
                existingCategory.Data.Name = questionsSubCategoryDto.Name;
                existingCategory.Data.UpdatedAt = DateTime.Now;
                var result = await _questionsSubCategoriesService.TUpdate(existingCategory.Data);
                if (result.Status == "success" && result.IsSuccess)
                {
                    return Ok(new { Status = 1, Message = "Kategori başarıyla güncellendi" });
                }
                return BadRequest(new { Status = 0, Message = result.Messages });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = 0, Message = ex.Message });
            }
        }

        [HttpPut]
        [Route("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingCategory = await _questionsSubCategoriesService.TGetById(id);
                if (existingCategory.Status != "success" || !existingCategory.IsSuccess || existingCategory.Data == null)
                {
                    return BadRequest(new { Status = 0, Message = "Kategori bulunamadı" });
                }
                var result = await _questionsSubCategoriesService.TDelete(id);
                if (result.Status == "success" && result.IsSuccess)
                {
                    return Ok(new { Status = 1, Message = "Kategori başarıyla silindi" });
                }
                return BadRequest(new { Status = 0, Message = result.Messages });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = 0, Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await _questionsSubCategoriesService.TGetById(id);
                if (result.Status == "success" && result.IsSuccess)
                {
                    return Ok(new { Status = 1, Data = result.Data });
                }
                return BadRequest(new { Status = 0, Message = result.Messages });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = 0, Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> List(int categoryId, string? categoryName, int? parentId, bool onlyRoot = false)
        {
            try
            {
                var result = await _questionsSubCategoriesService.TGetByCondition(x =>
                    x.CategoryId == categoryId && // Check CategoryId
                    (string.IsNullOrEmpty(categoryName) || x.Name.Contains(categoryName)) && // Check CategoryName
                    (!onlyRoot || x.IsRoot) && // Check onlyRoot
                    x.IsActive && // Ensure IsActive is true
                    (!parentId.HasValue || x.ParentId == parentId) // Check parentId
                );
                foreach (var item in result.Data)
                {
                    item.Parent = null;
                }
                if (result.Status == "success" && result.IsSuccess)
                {
                    return Ok(new { Status = 1, Data = result.Data });
                }
                return BadRequest(new { Status = 0, Message = result.Messages });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = 0, Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("list/tree")]
        public IActionResult ListTree(int categoryId)
        {
            try
            {
                var subCategories = _questionsSubCategoriesService.TGetAllSubCategoriesWithHierarchy(categoryId);
                return Ok(new { Status = 1, Data = subCategories });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = 0, Message = ex.Message });
            }
        }
    }

}