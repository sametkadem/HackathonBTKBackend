using AppBackend.DtoLayer.Dtos.QuestionsDto;
using AppBackend.EntityLayer.Concrete;
using AutoMapper;

namespace AppBackend.WebApi.Mapping
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<QuestionsCategoriesAddDto, QuestionsCategories>().ReverseMap();
            CreateMap<QuestionsCategoriesUpdateDto, QuestionsCategories>().ReverseMap();
            CreateMap<QuestionsAddDto, Questions>().ReverseMap();
            CreateMap<FeedbacksAddDto, Feedbacks>().ReverseMap();
        }
    }
}
