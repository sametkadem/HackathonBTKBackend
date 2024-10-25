using AppBackend.BusinessLayer.Abstract;
using AppBackend.DtoLayer.Dtos.QuestionsDto;
using AppBackend.EntityLayer.Concrete;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppBackend.WebApi.Controllers.QuestionsController
{
    [Route("api/v1/questions/category/")]
    [ApiController]
    public class QuestionsCategoryController : ControllerBase
    {
       
        private readonly IQuestionsCategoriesService _questionsCategoriesService;
        private readonly IMapper _mapper;
        
        public QuestionsCategoryController(IQuestionsCategoriesService questionsCategoriesService, IMapper mapper)
        {
                _questionsCategoriesService = questionsCategoriesService;
                _mapper = mapper;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] QuestionsCategoriesAddDto questionsCategoryDto)
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
                var questionsCategory = _mapper.Map<QuestionsCategories>(questionsCategoryDto);
                var isRoot = (questionsCategoryDto.ParentId == 0 || questionsCategoryDto.ParentId == null) ? true : false;
                var path = "";
                if (!isRoot)
                {             
                    var parent = await _questionsCategoriesService.TGetById(questionsCategoryDto.ParentId);
                    if(parent.Status != "success" || !parent.IsSuccess || parent.Data == null)
                    {
                        return BadRequest(new { Status = 0, Message = "Üst kategori bulunamadı" });
                    }   
                    path = parent.Data.Path;
                }
                path += (path == "") ? questionsCategoryDto.CategoryName : " -> " + questionsCategoryDto.CategoryName;
                questionsCategory.Name = questionsCategoryDto.CategoryName;
                questionsCategory.DisplayName = questionsCategoryDto.CategoryName;
                questionsCategory.IsActive = true;
                questionsCategory.ParentId = (isRoot) ? null : questionsCategoryDto.ParentId;
                questionsCategory.IsRoot = isRoot;
                questionsCategory.IsLeaf = (isRoot) ? false : true;
                questionsCategory.Path = path;
                questionsCategory.CreatedAt = DateTime.Now;
                questionsCategory.UpdatedAt = DateTime.Now;
                var result = await _questionsCategoriesService.TInsert(questionsCategory);
                if (result.Status == "success" && result.IsSuccess)
                {
                    return Ok(new { Status = 1, Message = "Kategori başarıyla eklendi" });
                }
                return BadRequest(new { Status = 0, Message = result.Messages, Data = result.Data});
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = 0, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] QuestionsCategoriesUpdateDto questionsCategoryDto)
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
                var existingCategory = await _questionsCategoriesService.TGetById(questionsCategoryDto.Id);
                if(existingCategory.Status != "success" || !existingCategory.IsSuccess || existingCategory.Data == null)
                {
                    return BadRequest(new { Status = 0, Message = "Kategori bulunamadı" });
                }
                existingCategory.Data.Name = questionsCategoryDto.Name;
                existingCategory.Data.UpdatedAt = DateTime.Now;
                var result = await _questionsCategoriesService.TUpdate(existingCategory.Data);
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
                var existingCategory = await _questionsCategoriesService.TGetById(id);
                if(existingCategory.Status != "success" || !existingCategory.IsSuccess || existingCategory.Data == null)
                {
                    return BadRequest(new { Status = 0, Message = "Kategori bulunamadı" });
                }              
                var result = await _questionsCategoriesService.TDelete(id);
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
                var result = await _questionsCategoriesService.TGetById(id);
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
        public async Task<IActionResult> List(string? categoryName, int? parentId, bool onlyRoot = false)
        {
            try
            {
                var result = await _questionsCategoriesService.TGetByCondition(x =>
                    (string.IsNullOrEmpty(categoryName) || x.Name == categoryName) && // Check CategoryName
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
        public IActionResult ListTree()
        {
            try
            {
                var subCategories = _questionsCategoriesService.TGetAllSubCategoriesWithHierarchy();
                return Ok(new { Status = 1, Data = subCategories });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = 0, Message = ex.Message });
            }
        }

    }
}
