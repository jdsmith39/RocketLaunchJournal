using System;
using System.Collections.Generic;
using System.Text;

namespace RocketLaunchJournal.Infrastructure.Services
{
    /// <summary>
    /// Base response class for services
    /// </summary>
    /// <typeparam name="T">Normally a Dto class</typeparam>
    public class BaseServiceResponse<T>
    {
        public BaseServiceResponse(T data)
        {
            Data = data;
        }

        public BaseServiceResponse(T data, System.Net.HttpStatusCode status)
        {
            Data = data;
            Status = status;
        }

        public T Data { get; set; }
        /// <summary>
        /// Status:  Using HTTP status codes  Defaults to OK
        /// </summary>
        public System.Net.HttpStatusCode Status { get; set; } = System.Net.HttpStatusCode.OK;
        public bool Succeeded { get { return Status == System.Net.HttpStatusCode.OK; } }
        public string Message { get; set; }

        public void UpdateResponseBasedOnResponse<Y>(BaseServiceResponse<Y> bsr)
        {
            Status = bsr.Status;
            Message = bsr.Message;
        }
    }
}
