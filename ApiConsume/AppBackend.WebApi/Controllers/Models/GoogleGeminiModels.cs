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
    public class GoogleGeminiModels
    {
        private readonly string _googleGeminiApiKey;
        private readonly string _googleGeminiApiUrl;
        private readonly string _text;
        private readonly IConfiguration _configuration;

        public GoogleGeminiModels(string text, IConfiguration configuration)
        {
            _configuration = configuration;
            _googleGeminiApiKey = _configuration["Google:Gemini:ApiKey"];
            _googleGeminiApiUrl = _configuration["Google:Gemini:ApiUrl"];
            _text = text;
        }

        public async Task<ResultDto> AnalyzeTextAsync()
        {           
            if (string.IsNullOrWhiteSpace(_text))
            {
                return new ResultDto
                {
                    Status = 0,
                    Message = "Text bulunamadı.",
                    Errors = null
                };
            }

            if (string.IsNullOrWhiteSpace(_googleGeminiApiKey))
            {
                return new ResultDto
                {
                    Status = 0,
                    Message = "Google Gemini Api Key bulunamadı.",
                    Errors = null
                };
            }

            if (string.IsNullOrWhiteSpace(_googleGeminiApiUrl))
            {
                return new ResultDto
                {
                    Status = 0,
                    Message = "Google Gemini Api Url bulunamadı.",
                    Errors = null
                };
            }

            using (var httpClient = new HttpClient())
            {
                var requestUri = _googleGeminiApiUrl + _googleGeminiApiKey;
                Console.WriteLine(requestUri);
                // İstek gövdesini belirle
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = _text + " " + " Bu soruyu çözermisin ? " }
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
                    JArray dataArray = (JArray)jsonObject["candidates"];
                    var firstElement = dataArray[0];
                    var stringresponse = "";
                    foreach (var item in firstElement["content"]["parts"])
                    {
                        stringresponse += item["text"] + " ";                     
                    }
                    if(stringresponse == null || stringresponse == "")
                    {
                        return new ResultDto
                        {
                            Status = 0,
                            Message = "Bir hata ile karşılaşıldı. Google Gemini Model responses bulumadı!.",
                            Errors = "Google Gemini Model - responses"
                        };
                    }
                    return new ResultDto
                    {
                        Status = 1,
                        Message = "İşlem başarılı.",
                        Data = stringresponse
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
