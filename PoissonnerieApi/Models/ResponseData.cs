using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoissonnerieApi.Models
{
    public class ResponseData
    {
        public static int CODE_SUCCESS = 0;
        public static int CODE_ERROR = -1;
        public static int CODE_ENQUETE_OK = -2;
        public static int CODE_EMPTY_QUESTION = -3;
        public static string MESSAGE_OK = "OK";

        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public object Sender { get; set; }

        public static ResponseData GetSuccess(object data, object sender = null)
        {
            var responseData = new ResponseData();
            responseData.ResponseCode = ResponseData.CODE_SUCCESS;
            responseData.Message = ResponseData.MESSAGE_OK;
            responseData.Data = data;
            responseData.Sender = sender;

            return responseData;
        }

        public static ResponseData GetError(string message, int codeErreur = -1, object data = null)
        {
            var responseData = new ResponseData();
            responseData.ResponseCode = codeErreur;
            responseData.Message = message;
            responseData.Data = data;

            return responseData;
        }
    }
}