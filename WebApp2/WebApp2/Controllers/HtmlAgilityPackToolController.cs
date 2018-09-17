using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace WebApp2.Controllers
{
    public class HtmlAgilityPackToolController : Controller
    {
        public async Task<IActionResult> Index()
        {
            //foreach (var item in GetWebPageData())
            //{
            //    await Response.Body.WriteAsync(Encoding.Default.GetBytes(item));
            //}
            //await Response.Body.WriteAsync(Encoding.Default.GetBytes("123"));
            return View(await GetWebPageData());
        }

        private async Task<List<string>> GetWebPageData()
        {
            ////操作 逻辑 ：
            //1.获取col-md-4 的div 列表
            //2.遍历col获取标题和链接

            List<string> colList = new List<string>();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("http://www.gongjuji.net");
            string rowPath = "/html/body/div[2]/div[2]/div";

            HtmlNodeCollection cols = doc.DocumentNode.SelectNodes(rowPath);

            foreach (var item in cols)
            {
                //解析 内部的 .thumbnail内容
                HtmlNode thumbnail = HtmlNode.CreateNode(item.InnerHtml);
                // 获取h3的内容和a标签 的链接
                HtmlNode h3 = thumbnail.SelectSingleNode("//h3");
                HtmlNode a = thumbnail.SelectSingleNode("//a");
                colList.Add(h3.InnerText + ": " + a.Attributes["href"].Value);
            }
            return await Task.FromResult(colList);
        }
    }
}