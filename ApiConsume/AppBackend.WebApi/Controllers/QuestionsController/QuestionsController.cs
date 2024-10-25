using AppBackend.BusinessLayer.Abstract;
using AppBackend.DtoLayer.Dtos.QuestionsDto;
using AppBackend.EntityLayer.Concrete;
using AppBackend.EntityLayer.Concrete.Identity;
using AppBackend.WebApi.Controllers.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppBackend.WebApi.Controllers.QuestionsController
{
    [Authorize]
    [Route("api/v1/questions/")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private static IWebHostEnvironment _webHostEnvironment;
        private readonly IQuestionsService _questionsService;
        private readonly IAnswersService _answersService;
        private readonly IQuestionsCategoriesService _questionsCategoriesService;
        private readonly IConfiguration _configuration;

        public QuestionsController(UserManager<AppUser> userManager, IMapper mapper, IWebHostEnvironment webHostEnvironment, IQuestionsService questionsService, IAnswersService answersService, IQuestionsCategoriesService questionsCategoriesService, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _questionsService = questionsService;
            _answersService = answersService;
            _questionsCategoriesService = questionsCategoriesService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddQuestionAsync([FromForm] QuestionsAddDto questionsAddDto)
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
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
                if (userIdClaim == null)
                {
                    return BadRequest(new { Status = 0, Message = "Kullanıcı ID bulunamadı." });
                }
                var user = await _userManager.FindByIdAsync(userIdClaim.Value);
                if (user == null)
                {
                    return BadRequest(new { Status = 0, Message = "Kullanıcı bulunamadı." });
                }
                if((questionsAddDto.QuestionImage == null || questionsAddDto.QuestionImage.Length == 0) && (questionsAddDto.Question == null || questionsAddDto.Question == "" || questionsAddDto.Question == "string"))
                {
                    return BadRequest(new { Status = 0, Message = "Soru veya resim yüklemesi yapmadınız." });
                }
                var files = questionsAddDto.QuestionImage;
                var filePath = "";
                var uploadControl = false;
                questionsAddDto.QuestionImage = null;
                var fileName = "";
                if (files != null && files.Length > 0)
                {                  
                    string path = _webHostEnvironment.WebRootPath + "\\uploads\\image\\questions\\";
                    //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/image/questions/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var randomGUID = Guid.NewGuid();
                    fileName = "QuestionImage_" + (randomGUID) + ".png";
                    filePath = Path.Combine(path, fileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    using (FileStream fileStream = System.IO.File.Create(filePath))
                    {
                        files.CopyTo(fileStream);
                        fileStream.Flush();
                        uploadControl = true;
                    }
                }
                var question = _mapper.Map<Questions>(questionsAddDto);
                if (uploadControl)
                {
                    question.QuestionFileName = fileName;
                    question.QuestionFilePath = filePath;
                    question.QuestionFileType = "image/png";
                    question.QuestionFileUrl = "/uploads/image/questions/" + fileName;
                }
                else
                {
                    question.QuestionFileName = null;
                    question.QuestionFilePath = null;
                    question.QuestionFileType = null;
                    question.QuestionFileUrl = null;
                }
                var category = await _questionsCategoriesService.TGetById(Convert.ToInt32(questionsAddDto.CategoryId));
                if (questionsAddDto.CategoryId == 0 || category.Status == "error" || !category.IsSuccess || category.Data == null)
                {
                    questionsAddDto.CategoryId = 1;
                    category = await _questionsCategoriesService.TGetById(Convert.ToInt32(questionsAddDto.CategoryId));
                    if (category.Status == "error" || !category.IsSuccess || category.Data == null)
                    {
                        return BadRequest(new { Status = 0, Message = "Kategori bulunamadı" });
                    }
                }
                var questionText = "";
                if (questionsAddDto.Question != null && questionsAddDto.Question != "" && questionsAddDto.Question != "string")
                {
                    questionText = questionsAddDto.Question;
                }
                else
                {
                    var googleVisionModel = new GoogleVisionModels(filePath, _configuration);
                    var result = await googleVisionModel.AnalyzeImageAsync();
                    if (result.Status == 1)
                    {
                        questionText = result.Data;
                    }
                    else
                    {
                        return BadRequest(new { Status = 0, Message = "Resim analizi yapılırken bir hata oluştu." + result.Message });
                    }
                }
                question.Question = questionText;
                question.Category = category.Data;
                question.CategoryId = category.Data.Id;
                question.UserId = user.Id;
                question.AppUser = user;
                question.IsApproved = true;
                question.CreatedAt = DateTime.Now;
                question.UpdatedAt = DateTime.Now;
                var googleGeminiModel = new GoogleGeminiModels(questionText, _configuration);
                var resultGemini = await googleGeminiModel.AnalyzeTextAsync();
                if (resultGemini.Status == 1)
                {
                    var data = resultGemini.Data;
                    question.Question = questionText;
                    var insertResult = await _questionsService.TInsert(question);
                    if (insertResult.IsSuccess && insertResult.Status == "success")
                    {
                        var answers = new Answers
                        {
                            Question = question,
                            QuestionId = insertResult.Data.Id,
                            UserId = user.Id,
                            AppUser = user,
                            Answer = resultGemini.Data,
                            IsApproved = true,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        var insertAnswerResult = await _answersService.TInsert(answers);
                        if (insertAnswerResult.IsSuccess && insertAnswerResult.Status == "success")
                        {
                            var item = insertResult.Data;
                            var answer = insertAnswerResult.Data;
                            var questionListDto = new QuestionsListDto
                            {
                                Id = item.Id,
                                QuestionImage = item.QuestionFileUrl,
                                CategoryName = category?.Data?.Name ?? "Bilinmiyor",
                                CategoryId = item.CategoryId,
                                CategoryPath = category?.Data?.Path ?? "Bilinmiyor",
                                CategoryParentId = category?.Data?.ParentId,
                                Question = item.Question,
                                CreatedAt = item.CreatedAt,
                                UpdatedAt = item.UpdatedAt,
                                Answers = new List<AnswerListDto>()
                            };

                        
                                var answerListDto = new AnswerListDto
                                {
                                    Id = answer.Id,
                                    Answer = answer.Answer,
                                    CreatedAt = answer.CreatedAt,
                                    UpdatedAt = answer.UpdatedAt
                                };
                                questionListDto.Answers.Add(answerListDto);
                            
                            return Ok(new { Status = 1, Message = "Soru ve cevap başarıyla eklendi.", Data = questionListDto });
                        }
                        else
                        {
                            var item = insertResult.Data;
                            var answer = insertAnswerResult.Data;
                            var questionListDto = new QuestionsListDto
                            {
                                Id = item.Id,
                                QuestionImage = item.QuestionFileUrl,
                                CategoryName = category?.Data?.Name ?? "Bilinmiyor",
                                CategoryId = item.CategoryId,
                                CategoryPath = category?.Data?.Path ?? "Bilinmiyor",
                                CategoryParentId = category?.Data?.ParentId,
                                Question = item.Question,
                                CreatedAt = item.CreatedAt,
                                UpdatedAt = item.UpdatedAt,
                                Answers = new List<AnswerListDto>()
                            };
                            return BadRequest(new { Status = 0, Message = "Cevap eklenirken bir hata oluştu.", Data = questionListDto });
                        }
                    }
                    else
                    {
                        return BadRequest(new { Status = 0, Message = "Soru eklenirken bir hata oluştu." });
                    }
                }


                return BadRequest(new { Status = 0, Message = "Soru eklenirken bir hata oluştu" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = 0, Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> List([FromQuery] QuestionListParamsDto questionListParamsDto)
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
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
                if (userIdClaim == null)
                {
                    return BadRequest(new { Status = 0, Message = "Kullanıcı ID bulunamadı." });
                }
                var user = await _userManager.FindByIdAsync(userIdClaim.Value);
                if (user == null)
                {
                    return BadRequest(new { Status = 0, Message = "Kullanıcı bulunamadı." });
                }
                var result = await _questionsService.TGetPaged(
                    questionListParamsDto.Page,
                    questionListParamsDto.PageSize,
                    x =>
                        x.UserId == user.Id &&
                        (questionListParamsDto.CategoryId == 0 || x.CategoryId == questionListParamsDto.CategoryId) &&
                        (string.IsNullOrEmpty(questionListParamsDto.Question) || x.Question.Contains(questionListParamsDto.Question))
                );
                if (!result.IsSuccess || result.Status == "error")
                {
                    return BadRequest(new { Status = 0, Message = result.Messages });
                }
                var questionList = new List<QuestionsListDto>();

                foreach (var item in result.Data.Data)
                {
                    var category = await _questionsCategoriesService.TGetById(item.CategoryId);
                    var answer = await _answersService.TGetByCondition(x => x.QuestionId == item.Id);

                    var questionListDto = new QuestionsListDto
                    {
                        Id = item.Id,
                        QuestionImage = item.QuestionFileUrl,
                        CategoryName = category?.Data?.Name ?? "Bilinmiyor",
                        CategoryId = item.CategoryId,
                        CategoryPath = category?.Data?.Path ?? "Bilinmiyor",
                        CategoryParentId = category?.Data?.ParentId,
                        Question = item.Question,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt,
                        Answers = new List<AnswerListDto>()
                    };

                    foreach (var answerItem in answer.Data)
                    {
                        var answerListDto = new AnswerListDto
                        {
                            Id = answerItem.Id,
                            Answer = answerItem.Answer,
                            CreatedAt = answerItem.CreatedAt,
                            UpdatedAt = answerItem.UpdatedAt
                        };
                        questionListDto.Answers.Add(answerListDto);
                    }

                    questionList.Add(questionListDto);
                }
                return Ok(new { Status = 1, Message = "İşlem başarılı",
                    TotalCount = result.Data.TotalCount,
                    TotalPages = result.Data.TotalPages,
                    CurrentPage = result.Data.CurrentPage,
                    PageSize = result.Data.PageSize,
                    HasNextPage = result.Data.HasNextPage,
                    Data = questionList,
  
                });
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
                var existingQuestion = await _questionsService.TGetById(id);
                if (existingQuestion.Status != "success" || !existingQuestion.IsSuccess || existingQuestion.Data == null)
                {
                    return BadRequest(new { Status = 0, Message = "Soru bulunamadı" });
                }
                var filepath = existingQuestion.Data.QuestionFilePath;
                if (filepath != null && filepath != "")
                {
                    if (System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
                }
                var result = await _questionsService.TDelete(id);
                if (result.Status == "success" && result.IsSuccess)
                {
                    return Ok(new { Status = 1, Message = "Soru başarıyla silindi" });
                }
                return BadRequest(new { Status = 0, Message = result.Messages });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = 0, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateQuestions([FromBody] QuestionsUpdateDto questionsUpdateDto)
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
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
                if (userIdClaim == null)
                {
                    return BadRequest(new { Status = 0, Message = "Kullanıcı ID bulunamadı." });
                }
                var user = await _userManager.FindByIdAsync(userIdClaim.Value);
                if (user == null)
                {
                    return BadRequest(new { Status = 0, Message = "Kullanıcı bulunamadı." });
                }
                var existingQuestion = await _questionsService.TGetById(questionsUpdateDto.Id);
                if (existingQuestion.Status != "success" || !existingQuestion.IsSuccess || existingQuestion.Data == null)
                {
                    return BadRequest(new { Status = 0, Message = "Soru bulunamadı" });
                }
                var existingCategory = await _questionsCategoriesService.TGetById(Convert.ToInt32(questionsUpdateDto.CategoryId));
                if (questionsUpdateDto.CategoryId != 0 && (existingCategory.Status == "error" || !existingCategory.IsSuccess || existingCategory.Data == null))
                {
                    return BadRequest(new { Status = 0, Message = "Kategori bulunamadı" });
                }
                existingQuestion.Data.CategoryId = existingCategory.Data.Id;
                if(questionsUpdateDto.Question != null && questionsUpdateDto.Question != "" && questionsUpdateDto.Question != "string")
                {
                 
                    var googleGeminiModel = new GoogleGeminiModels(questionsUpdateDto.Question, _configuration);
                    var resultGemini = await googleGeminiModel.AnalyzeTextAsync();
                    if (resultGemini.Status == 1)
                    {
                        var answers = new Answers
                        {
                            Question = existingQuestion.Data,
                            QuestionId = existingQuestion.Data.Id,
                            UserId = user.Id,
                            AppUser = user,
                            Answer = resultGemini.Data,
                            IsApproved = true,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        };
                        var insertAnswerResult = await _answersService.TInsert(answers);
                        if(insertAnswerResult.IsSuccess && insertAnswerResult.Status == "success")
                        {
                            existingQuestion.Data.Question = questionsUpdateDto.Question;
                        }
                        else
                        {
                            return BadRequest(new { Status = 0, Message = "Cevap eklenirken bir hata oluştu." });
                        }
                    }
                    else
                    {
                        return BadRequest(new { Status = 0, Message = "Soru analizi yapılırken bir hata oluştu." + resultGemini.Message });
                    }
                }
                existingQuestion.Data.UpdatedAt = DateTime.Now;
                var result = await _questionsService.TUpdate(existingQuestion.Data);
                if (result.Status == "success" && result.IsSuccess)
                {
                    var item = result.Data;
                    var answer = await _answersService.TGetByCondition(x => x.QuestionId == item.Id);                  
                    var questionListDto = new QuestionsListDto
                    {
                        Id = item.Id,
                        QuestionImage = item.QuestionFileUrl,
                        CategoryName = existingCategory?.Data?.Name ?? "Bilinmiyor",
                        CategoryId = item.CategoryId,
                        CategoryPath = existingCategory?.Data?.Path ?? "Bilinmiyor",
                        CategoryParentId = existingCategory?.Data?.ParentId,
                        Question = item.Question,
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt,
                        Answers = new List<AnswerListDto>()
                    };
                    foreach (var answerItem in answer.Data)
                    {
                        var answerListDto = new AnswerListDto
                        {
                            Id = answerItem.Id,
                            Answer = answerItem.Answer,
                            CreatedAt = answerItem.CreatedAt,
                            UpdatedAt = answerItem.UpdatedAt
                        };
                        questionListDto.Answers.Add(answerListDto);
                    }
                    return Ok(new { Status = 1, Message = "Soru başarıyla güncellendi", Data = questionListDto });
                }
                return BadRequest(new { Status = 0, Message = result.Messages });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = 0, Message = ex.Message });
            }
        }

    }
}
