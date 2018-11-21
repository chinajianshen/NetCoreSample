using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FclConsoleApp.FileOperation
{
    /// <summary>
    /// 异步下载文件
    /// </summary>
    public class DownLoadFileAsync
    {
        private string CurrDirectoryPath;
        public DownLoadFileAsync()
        {
            CurrDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "WebDirectory");
            if (!Directory.Exists(CurrDirectoryPath))
            {
                Directory.CreateDirectory(CurrDirectoryPath);
            }
        }

        public  void WebDownLoadFileAsync(string url)
        {
            try
            {
                string loadFileName = Path.Combine(CurrDirectoryPath, $"{DateTime.Now.Ticks}.txt");
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                RequestState requestState = new RequestState();
                requestState.Request = httpWebRequest;
                requestState.Savepath = loadFileName;
                requestState.FileStream = File.Create(loadFileName);
                httpWebRequest.BeginGetResponse(new AsyncCallback(ResponseCallBack), requestState);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Message is :{0}", ex.Message);
            }
        }


        public void ResponseCallBack(IAsyncResult callbackResult)
        {
            RequestState myRequestState = callbackResult.AsyncState as RequestState;

            HttpWebRequest myHttpRequest = myRequestState.Request;

            myRequestState.Response = (HttpWebResponse)myHttpRequest.EndGetResponse(callbackResult);

            Stream responseStream = myRequestState.Response.GetResponseStream();
            myRequestState.StreamResponse = responseStream;

            IAsyncResult asynchronousRead = responseStream.BeginRead(myRequestState.BufferRead, 0, myRequestState.BufferRead.Length, ReadCallBack, myRequestState);
        }

        public void ReadCallBack(IAsyncResult asyncResult)
        {
            RequestState myRequestState = asyncResult.AsyncState as RequestState;

            Stream responseStream = myRequestState.StreamResponse;

            int readSize = responseStream.EndRead(asyncResult);
            if (readSize > 0)
            {
                myRequestState.FileStream.Write(myRequestState.BufferRead, 0, readSize);
                responseStream.BeginRead(myRequestState.BufferRead, 0, myRequestState.BufferRead.Length, ReadCallBack, myRequestState);
            }
            else
            {
                Console.WriteLine("\nThe Length of the File is: {0}", myRequestState.FileStream.Length);
                Console.WriteLine("DownLoad Completely, Download path is: {0}", myRequestState.Savepath);
                myRequestState.Response.Close();
                myRequestState.FileStream.Close();
            }
        }
    }

    public class RequestState
    {
        public RequestState()
        {
            BufferRead = new byte[1024];
        }

        public HttpWebRequest Request { get; set; }

        public HttpWebResponse Response { get; set; }

        public Stream StreamResponse { get; set; }

        public byte[] BufferRead { get; set; }

        public FileStream FileStream { get; set; }

        public string Savepath { get; set; }
    }
}
