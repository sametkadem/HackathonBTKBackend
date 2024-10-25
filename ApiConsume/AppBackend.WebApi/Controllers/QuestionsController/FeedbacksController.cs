using AppBackend.BusinessLayer.Abstract;
using AppBackend.DtoLayer.Dtos.QuestionsDto;
using AppBackend.EntityLayer.Concrete;
using AppBackend.EntityLayer.Concrete.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppBackend.WebApi.Controllers.QuestionsController
{
    [Authorize]
    [Route("api/v1/questions/feedbacks")]
    public class FeedbacksController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IQuestionsService _questionsService;
        private readonly IFeedbacksService _feedbacksService;

        public FeedbacksController(UserManager<AppUser> userManager, IMapper mapper, IQuestionsService questionsService, IFeedbacksService feedbacksService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _questionsService = questionsService;
            _feedbacksService = feedbacksService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add([FromBody] FeedbacksAddDto feedbackDto)
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
                var feedback = _mapper.Map<Feedbacks>(feedbackDto);
                if(feedbackDto.QuestionId != null && feedbackDto.QuestionId != 0)
                {
                    var question = await _questionsService.TGetById(Convert.ToInt32(feedbackDto.QuestionId));
                    if (question.Status != "success" || !question.IsSuccess || question.Data == null)
                    {
                        return BadRequest(new { Status = 0, Message = "Soru bulunamadı" });
                    }
                    feedback.QuestionId = question.Data.Id;
                }
                else
                {
                    feedback.QuestionId = null;
                }                
                feedback.UserId = user.Id;
                feedback.CreatedAt = DateTime.Now;
                feedback.UpdatedAt = DateTime.Now;
                var result = await _feedbacksService.TInsert(feedback);
                if (result.Status != "success" || !result.IsSuccess || result.Data == null)
                {
                    return BadRequest(new { Status = 0, Message = "Geri bildirim eklenemedi" });
                }
                return Ok(new { Status = 1, Message = "Geri bildirim eklendi", Data = result.Data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Status = 0, Message = ex.Message });
            }
        }
    }
}
