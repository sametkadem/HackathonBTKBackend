using AppBackend.BusinessLayer.Abstract;
using AppBackend.BusinessLayer.Concrete;
using AppBackend.DtoLayer.Dtos.QuestionsDto;
using AppBackend.EntityLayer.Concrete;
using AppBackend.EntityLayer.Concrete.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppBackend.WebApi.Controllers.QuestionsController
{
    [Route("api/v1/questions/")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private static IWebHostEnvironment _webHostEnvironment;
        private readonly IQuestionsService _questionsService;
        private readonly IQuestionsCategoriesService _questionsCategoriesService;

        public QuestionsController(UserManager<AppUser> userManager, IMapper mapper, IWebHostEnvironment webHostEnvironment, IQuestionsService questionsService, IQuestionsCategoriesService questionsCategoriesService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _questionsService = questionsService;
            _questionsCategoriesService = questionsCategoriesService;
        }

        private class RandomStringGenerator
        {
            private static Random random = new Random();
            public static string GenerateRandomString(int length)
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                return new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }
        /*
        [HttpPost]
        [Route("add")]
        public IActionResult Add([FromForm] QuestionsAddDto questionsAddDto)
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
               
                var files = questionsAddDto.QuestionImage;
                var filePath = "";
                var uploadControl = false;
                questionsAddDto.QuestionImage = null;
                var fileName = "";
                if (files != null && files.Length > 0)
                {
                    string path = _webHostEnvironment.WebRootPath + "\\uploads\\image\\questions\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    int length = 60;
                    string randomString = RandomStringGenerator.GenerateRandomString(length);
                    fileName = "QuestionImage_" + (randomString) + ".png";
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
                question.UserId = int.Parse(userIdClaim.Value);
                question.IsApproved = true;
                question.CreatedAt = DateTime.Now;
                question.UpdatedAt = DateTime.Now;
                question.CategoryId = questionsAddDto.CategoryId;
                question.Question = questionsAddDto.Question;
                var insertResult = _questionsService.TInsert(question);
                if(insertResult.ıS


                return BadRequest(new { Status = 0, Message = "Soru eklenirken bir hata oluştu" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = 0, Message = ex.Message });
            }
        }
*/
    }
}
