using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using CommLib;

namespace WebApiKit
{
    public enum EnuHttpMethod
    {
        Get,
        Put,
        Post,
        Delete
    }

    /// <summary>
    /// 这是自定义的RESTFul Web API的客户端
    /// 通过Custom-Auth-Name和Custom-Auth-Key来识别用户身份
    /// </summary>
    public static class WebApiClientHelper
    {
        public static string UserName { get; set; }
        public static string Password { get; set; }

        static readonly HttpClient s_httpClient = new HttpClient();

        private static void MakePrincipleHeader(HttpRequestMessage reqMsg, string strUri)
        {
            Guid guid = Guid.NewGuid();
            strUri = InternalHelper.GetEffectiveUri(strUri);
            string strToEncrypt = Md5.MD5Encode(strUri + guid) + " " + guid;
            string strTheAuthKey = Des.Encode(strToEncrypt, Md5.MD5TwiceEncode(Password));
            reqMsg.Headers.Add(Consts.HTTP_HEADER_AUTH_USER, UserName);
            reqMsg.Headers.Add(Consts.HTTP_HEADER_AUTH_KEY, strTheAuthKey);
        }

        private static readonly MediaTypeFormatter[] s_formatters = new MediaTypeFormatter[] { new JsonMediaTypeFormatter() };

        public static string MakeConfidentialMessage(string strToEnc)
        {
            return Des.Encode(strToEnc, Md5.MD5TwiceEncode(Password));
        }

        public static T DoJsonRequest<T>(string strUri, EnuHttpMethod method, IUriConvertable queryCondition = null, Object objToSend = null, long tick = 0)
        {
            string strToRequest = strUri;

            if (tick != 0 && (method == EnuHttpMethod.Put || method == EnuHttpMethod.Delete))
                strToRequest += "?UpdateTicks=" + tick.ToString(CultureInfo.InvariantCulture);

            if (queryCondition != null && method == EnuHttpMethod.Get)
                strToRequest += queryCondition.QueryString;

            HttpRequestMessage requestMsg = new HttpRequestMessage();
            MakePrincipleHeader(requestMsg, strToRequest);
            requestMsg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMsg.RequestUri = new Uri(strToRequest);

            HttpContent content = null;

            if (objToSend != null)
                content = new StringContent(JsonConvert.SerializeObject(objToSend), Encoding.UTF8, "application/json");

            switch (method)
            {
                case EnuHttpMethod.Post:
                    requestMsg.Method = HttpMethod.Post;
                    requestMsg.Content = content;
                    break;
                case EnuHttpMethod.Put:
                    requestMsg.Method = HttpMethod.Put;
                    requestMsg.Content = content;
                    break;
                case EnuHttpMethod.Delete:
                    requestMsg.Method = HttpMethod.Delete;
                    break;
                default: //EnuHttpMethod.Get:
                    requestMsg.Method = HttpMethod.Get;
                    break;
            }

            Task<HttpResponseMessage> rtnAll = s_httpClient.SendAsync(requestMsg);

            #region 执行
            HttpResponseMessage resultMessage = null;
            Task<T> rtnFinal;

            try
            {
                try
                {
                    resultMessage = rtnAll.Result;
                }
                catch (AggregateException ae)
                {
                    foreach (var ex in ae.InnerExceptions)
                    {
                        if (ex is HttpRequestException)
                        {
                            throw new ClientException(ERRMSG_FAILED_TO_SEND_MESSAGE, ex);
                        }
                    }
                }

                if (!resultMessage.IsSuccessStatusCode)
                {
                    Task<WebApiHttpInfo> error;
                    try
                    {
                        error = resultMessage.Content.ReadAsAsync<WebApiHttpInfo>(s_formatters);
                    }
                    catch (Exception ex)
                    {
                        throw new ClientException(ERRMSG_SERVER_EXCEPTIONAL_FAILURE, ex);
                    }
                    throw new ClientException(error.Result.Message);
                }

                if (method != EnuHttpMethod.Get)
                {
                    return default(T);
                }

                try
                {
                    rtnFinal = resultMessage.Content.ReadAsAsync<T>(s_formatters);
                }
                catch (Exception ex)
                {
                    throw new ClientException(ERRMSG_RESULT_CONTENT_DOESNOT_MATCH, ex);
                }
            }
            catch (ClientException ex)
            {
                ex.ParameterObject = new { Uri = strUri, Method = method, Data = objToSend, Tick = tick };
                throw;
            }

            #endregion

            return rtnFinal.Result;
        }

        public static Stream DoStreamRequest(string strUri, EnuHttpMethod method, Stream stmToSend=null, long tick = 0)
        {
            string strToRequest = strUri;

            if (tick != 0 && (method == EnuHttpMethod.Put || method == EnuHttpMethod.Delete))
                strToRequest += "?UpdateTicks=" + tick.ToString(CultureInfo.InvariantCulture);

            HttpRequestMessage requestMsg = new HttpRequestMessage();
            MakePrincipleHeader(requestMsg, strToRequest);
            requestMsg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMsg.RequestUri = new Uri(strToRequest);

            HttpContent content = stmToSend!=null ? new StreamContent(stmToSend) : null;

            switch (method)
            {
                case EnuHttpMethod.Post:
                    requestMsg.Method = HttpMethod.Post;
                    requestMsg.Content = content;
                    break;
                case EnuHttpMethod.Put:
                    requestMsg.Method = HttpMethod.Put;
                    requestMsg.Content = content;
                    break;
                case EnuHttpMethod.Delete:
                    requestMsg.Method = HttpMethod.Delete;
                    break;
                default: //EnuHttpMethod.Get:
                    requestMsg.Method = HttpMethod.Get;
                    break;
            }

            Task<HttpResponseMessage> rtnAll = s_httpClient.SendAsync(requestMsg);

            #region 执行
            HttpResponseMessage resultMessage = null;
            Task<Stream> rtnFinal;
            try
            {
                try
                {
                    resultMessage = rtnAll.Result;
                }
                catch (AggregateException ae)
                {
                    foreach (var ex in ae.InnerExceptions)
                    {
                        if (ex is HttpRequestException)
                        {
                            throw new ClientException(ERRMSG_FAILED_TO_SEND_MESSAGE, ex);
                        }
                    }
                }

                if (!resultMessage.IsSuccessStatusCode)
                {
                    Task<WebApiHttpInfo> error;
                    try
                    {
                        error = resultMessage.Content.ReadAsAsync<WebApiHttpInfo>(s_formatters);
                    }
                    catch (Exception ex)
                    {
                        throw new ClientException(ERRMSG_SERVER_EXCEPTIONAL_FAILURE, ex);
                    }
                    throw new ClientException(error.Result.Message);
                }

                if (method != EnuHttpMethod.Get)
                {
                    return null;
                }

                try
                {
                    rtnFinal = resultMessage.Content.ReadAsStreamAsync();
                }
                catch (Exception ex)
                {
                    throw new ClientException(ERRMSG_RESULT_CONTENT_DOESNOT_MATCH, ex);
                }
            }
            catch (ClientException ex)
            {
                ex.ParameterObject = new { Uri = strUri, Method = method, Tick = tick };
                throw;
            }

            #endregion
            if (rtnFinal.Result.Length == 0)
                return null;
            return rtnFinal.Result;
        }

        private const string ERRMSG_FAILED_TO_SEND_MESSAGE = "发送网络请求失败";
        private const string ERRMSG_SERVER_EXCEPTIONAL_FAILURE = "服务器意外错误";
        private const string ERRMSG_RESULT_CONTENT_DOESNOT_MATCH = "服务器返回与请求不匹配";
    }
}
