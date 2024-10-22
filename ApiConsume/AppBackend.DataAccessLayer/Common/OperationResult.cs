using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DataAccessLayer.Common
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Status { get; set; } = "success";
        public int StatusCode { get; set; } = 200;
        public List<string> Messages { get; set; } = new();
        public T? Data { get; set; }

        public string Type { get; set; } = string.Empty;

        public OperationResult() { }

        public static OperationResult<T> Success(T data, string message = "", string type = "")
        {
            return new OperationResult<T>
            {
                StatusCode = 200,
                IsSuccess = true,
                Status = "success",
                Data = data,
                Messages = new List<string> { message },
                Type = type
            };
        }

        public static OperationResult<T> Failure(string message, string type = "")
        {
            return new OperationResult<T>
            {
                StatusCode = 500,
                IsSuccess = false,
                Status = "error",
                Messages = new List<string> { message },
                Type = type
            };
        }
    }
}
