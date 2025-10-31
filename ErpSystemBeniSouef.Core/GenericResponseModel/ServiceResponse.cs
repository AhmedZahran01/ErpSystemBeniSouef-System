using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSystemBeniSouef.Core.GenericResponseModel
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Operation successful";
        public T Data { get; set; }

        public static ServiceResponse<T> SuccessResponse(T data, string message = null)
        {
            return new ServiceResponse<T>
            {
                Data = data,
                Message = message ?? "Success",
                Success = true
            };
        }

        public static ServiceResponse<T> Failure(string message)
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }
    }

   

}
