using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Model.Common
{
    public class Response<T>
    {
        public Response()
        {
            StatusCode = HttpStatusCode.OK;
            Message = new List<string>();
            RequestTime = DateTime.Now;
        }
        public Response(T data, string message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Data = data;
            StatusCode = statusCode;
            Message = new List<string>();
            if (!string.IsNullOrEmpty(message))
                Message.Add(message);
            else
                Message.Add("Success");
            RequestTime = DateTime.Now;
        }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> Message { get; set; }
        public T Data { get; set; }
        public DateTime RequestTime { get; set; }
    }
}
