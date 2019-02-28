using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Server.Filters;
using Server.Helper;
using System.IO;

namespace Server.Controllers
{
    [WebApiAuthFilter]
    public class AvatarsController : ApiController
    {
        private string GetAvatarFullPath(string id)
        {
            return AppDomain.CurrentDomain.BaseDirectory + "avatars\\" + id + ".jpg";
        }

        // GET api/avatars/{username}
        // 获取头像（非管理员只能获取自己的）
        public HttpResponseMessage Get(string id)
        {
            this.CheckUserName(id);
            this.CheckAdministrator(id);

            HttpResponseMessage resp = Request.CreateResponse(HttpStatusCode.OK);
            string theAvatarFullPath = GetAvatarFullPath(id);
            if (!File.Exists(theAvatarFullPath))
                return resp;
            FileStream fs = File.Open(theAvatarFullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            resp.Content = new StreamContent(fs);
            resp.Content.Headers.Add("Content-Type", "image/jpeg");
            resp.Content.Headers.Add(WebApiKit.Consts.HTTP_HEADER_UPDATETICKS, File.GetLastWriteTime(theAvatarFullPath).Ticks.ToString(CultureInfo.InvariantCulture));
            return resp;
        }

        // PUT api/avatars/{username}
        // 更新头像（非管理员只能更新自己的）
        // 严格上来说，还是要做并行写入冲突检查
        public void Put(string id)
        {
            this.CheckUserName(id);
            this.CheckAdministrator(id);

            /* 注意，这里我直接调用了ReadAsStreamAsync，对CS程序是没有问题的，但对于浏览器程序（HTML+Javascript）来说有问题
             * 对于浏览器程序，文件这种二进制格式是随着表单通过Multipart Content这种内容类型来发送的，所以应该这样写：
             * 
             * // Verify that this is an HTML Form file upload request
             * if (!Request.Content.IsMimeMultipartContent("form-data"))
             * {
             *     throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.UnsupportedMediaType));
             * }
             * IEnumerable<HttpContent> parts = null;
             * 
             * //使用TaskCreationOptions.LongRunning选项可以避免可能发生的死锁，这个有待研究，死锁并非一定出现
             * Task.Factory
             *     .StartNew(() => parts = Request.Content.ReadAsMultipartAsync().Result.Contents,
             *         CancellationToken.None,
             *         TaskCreationOptions.LongRunning,
             *         TaskScheduler.Default)
             *     .Wait();
             * 
             *   foreach (var part in parts)
             *   {
             *       if(part.Headers.ContentDisposition.Name=="filename") //filename是客户端往Multipart Content中加入的项目名称
             *       {
             *           Task<Stream> tsk_stm = part.ReadAsStreamAsync();
             *           Stream stm = tsk_stm.Result;
             *           
             *           //这里执行进一步处理吧
             *       }
             *   }
             *   
             * 以上这种写法对应的Client的写法是：
             * MultipartFormDataContent formData = new MultipartFormDataContent();
             * using (FileStream fileStream = new FileStream("hugeblank.zero", FileMode.Open, FileAccess.Read, FileShare.Read))
             * {
             *     StreamContent content = new StreamContent(fileStream);
             *     formData.Add(content, "filename", "testfile");
             * }
             * httprequest.Content = formData;
             */
            Task<Stream> tsk_stm = Request.Content.ReadAsStreamAsync();
            Stream stm = tsk_stm.Result;
            using (FileStream fsToWrite = new FileStream( GetAvatarFullPath(id), FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                stm.CopyTo(fsToWrite);
            }
        }

        // DELETE api/avatars/{username}
        // 删除头像（非管理员只能删除自己的）
        // 严格上来说，还是要做并行写入冲突检查
        public void Delete(string id)
        {
            this.CheckUserName(id);
            this.CheckAdministrator(id);
            File.Delete(GetAvatarFullPath(id));
        }
    }
}
