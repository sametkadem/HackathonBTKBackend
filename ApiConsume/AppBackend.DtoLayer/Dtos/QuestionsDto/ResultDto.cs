using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DtoLayer.Dtos.QuestionsDto
{
    public class ResultDto
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public string Errors { get; set; }
        public string Data { get; set; } // JSON verisini temsil etmek için JToken kullanılıyor

      
    }
}
