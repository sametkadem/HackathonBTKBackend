using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AppBackend.DtoLayer.Dtos.QuestionsDto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AppBackend.WebApi.Controllers.Models
{
    public class GoogleVisionModels
    {
        private readonly string _googleVisionApiKey;
        private readonly string _googleVisionApiUrl;
        private readonly string _imagePath;
        private readonly IConfiguration _configuration;
        public GoogleVisionModels(string imagePath, IConfiguration configuration)
        {
            _googleVisionApiKey = configuration["Google:Vision:ApiKey"];
            _googleVisionApiUrl = configuration["Google:Vision:ApiUrl"];
            _imagePath = imagePath;
        }

        public async Task<ResultDto> AnalyzeImageAsync()
        {
            if (!File.Exists(_imagePath))
            {
                return new ResultDto
                {
                    Status = 0,
                    Message = "Resim bulunamadı.",
                    Errors = null
                };
            }

            using (var httpClient = new HttpClient())
            {
                var requestUri = _googleVisionApiUrl + _googleVisionApiKey;

                var requestBody = new
                {
                    requests = new[]
                    {
                        new
                        {
                            image = new
                            {
                                content = Convert.ToBase64String(File.ReadAllBytes(_imagePath))
                            },
                            features = new[]
                            {
                                new { type = "TEXT_DETECTION"}
                            }
                        }
                    }
                };
                try
                {
                    var json = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(requestUri, content);
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(responseBody);
                    JArray dataArray = (JArray)jsonObject["responses"];
                    var firstElement = dataArray[0];
                    if(firstElement == null)
                    {
                        return new ResultDto
                        {
                            Status = 0,
                            Message = "Bir hata ile karşılaşıldı. Google Vision Model responses bulumadı!.",
                            Errors = "Google Vision Model - responses"
                        };
                    }
                    if (firstElement["fullTextAnnotation"] == null || firstElement["fullTextAnnotation"]["text"] == null)
                    {
                        return new ResultDto
                        {
                            Status = 0,
                            Message = "Bir hata ile karşılaşıldı. Google Vision Model fullTextAnnotation bulumadı!.",
                            Errors = "Google Vision Model - fullTextAnnotation"
                        };
                    }                 
                    return new ResultDto
                    {
                         Status = 1,
                         Message = "İstek Başarılı.",
                         Data = (string)firstElement["fullTextAnnotation"]["text"]
                    };                  
                }
                catch (Exception ex)
                {
                    return new ResultDto
                    {
                        Status = 0,
                        Message = "Bir hata oluştu: " + ex.Message,
                        Errors = ex.ToString()
                    };
                }
            }
        }
    }
}
