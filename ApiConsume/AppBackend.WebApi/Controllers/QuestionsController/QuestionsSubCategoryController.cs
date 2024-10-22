using AppBackend.BusinessLayer.Abstract;
using AppBackend.DtoLayer.Dtos.QuestionsDto;
using AppBackend.EntityLayer.Concrete;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppBackend.WebApi.Controllers.QuestionsController
{
    [Route("api/v1/questions/category/sub/")]
    [ApiController]
    public class QuestionsSubCategoryController : ControllerBase
    {

        private readonly IQuestionsCategoriesService _questionsCategoriesService;
        private readonly IMapper _mapper;

        public QuestionsSubCategoryController(IQuestionsCategoriesService questionsCategoriesService, IMapper mapper)
        {
            _questionsCategoriesService = questionsCategoriesService;
            _mapper = mapper;
        }
        /*
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] QuestionsSubCategoriesAddDto createQuestionsSubCategoryDto)
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
                
                var existingCategory = await _questionsCategoriesService.TExists(
                           x => x.Name == createQuestionsSubCategoryDto.SubCategoryName && x.CategoryId == createQuestionsSubCategoryDto.CategoryId
                );
                if (existingSubCategory.Status == "error" || !existingSubCategory.IsSuccess || existingSubCategory.Data)
                {
                    return BadRequest(new { Status = 0, Message = existingSubCategory.Messages, Data = existingSubCategory});
                }
                var subCategory = _mapper.Map<QuestionsSubCategories>(createQuestionsSubCategoryDto);
                subCategory.CategoryId = createQuestionsSubCategoryDto.CategoryId;
                var parentId = createQuestionsSubCategoryDto.ParentId;
                if(parentId != null && parentId == 0)
                {
                    parentId = null;
                }
                else
                {
                    var existingSubCategoryParent = await _questionsSubCategoriesService.TExists(x => x.Id == parentId);
                    if(existingSubCategoryParent.Status == "error" || !existingSubCategoryParent.IsSuccess || !existingSubCategoryParent.Data)
                    {
                        return BadRequest(new { Status = 0, Message = existingSubCategoryParent.Messages, Data = existingSubCategoryParent });
                    }
                }
                subCategory.ParentId = parentId;
                subCategory.CreatedAt = DateTime.Now;
                subCategory.UpdatedAt = DateTime.Now;
                var result = await _questionsSubCategoriesService.TInsert(subCategory);
                if(result.IsSuccess && result.Status == "success")
                {
                    return Ok(new { Status = 1, Message = "Alt kategori başarıyla eklendi" });
                }
                return BadRequest(new { Status = 0, Message = result.Messages, Data = result.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = 0, Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var subCategory = await _questionsCategoriesService.TGetById(id);
                if (subCategory == null)
                {
                    return BadRequest(new { Status = 0, Message = "Alt kategori bulunamadı" });
                }
                return Ok(new { Status = 1, Data = subCategory.Data });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = 0, Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> List()
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
        */
    }
}
