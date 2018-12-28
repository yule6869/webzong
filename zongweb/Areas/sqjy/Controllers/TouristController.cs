using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sqjymodel;
using WeiXinKaifa;

namespace zongweb.Areas.sqjy.Controllers
{
    public class TouristController : Controller
    {
        public static string mystr=ConfigurationManager.AppSettings["ConnectionString2"].ToString();
        public static List<XinWen> zuixinxinwens = Utils.SqlstrToModels<XinWen>("select top 5 * from xinwen where shifouxianshi=1 order by shijian desc", mystr);
        public static List<KeCheng> zuixinkechengs = Utils.SqlstrToModels<KeCheng>("select top 5 * from kecheng where zhiding=0 and shifouxianshi=1 order by shangchuanshijian desc", mystr);
        // GET: sqjy/Tourist
        public ActionResult Index()
        {
            
            List<XinWen> xinwens = new List<XinWen>();
            List<KeCheng>[] kechengs = new List<KeCheng>[6];

            List<XinWen> xw1 = Utils.SqlstrToModels<XinWen>("select top 6 * from xinwen where shifouxianshi=1 and leixing='社区新闻' order by shijian desc", mystr);
            List<XinWen> xw2 = Utils.SqlstrToModels<XinWen>("select top 6 * from xinwen where shifouxianshi=1 and leixing='上级文件' order by shijian desc", mystr);
            List<XinWen> xw3 = Utils.SqlstrToModels<XinWen>("select top 6 * from xinwen where shifouxianshi=1 and leixing='通知公告' order by shijian desc", mystr);
            xinwens= xw1.Union(xw2).Union(xw3).ToList<XinWen>();
            xinwens.Sort();
            ViewBag.xinwens = xinwens;

            List<KeCheng> kc1 = Utils.SqlstrToModels<KeCheng>("SELECT top 1 * FROM kecheng where zhiding=1 and kechengleixing='社区生活' and shifouxianshi=1 order by id desc", mystr);
            List<KeCheng> kc2 = Utils.SqlstrToModels<KeCheng>("SELECT top 8 * FROM kecheng where zhiding=0 and shifouxianshi=1 and kechengleixing='社区生活' order by id desc", mystr);

            List<KeCheng> kc3 = Utils.SqlstrToModels<KeCheng>("SELECT top 1 * FROM kecheng where zhiding=1 and kechengleixing='科学探秘' and shifouxianshi=1 order by id desc", mystr);
            List<KeCheng> kc4 = Utils.SqlstrToModels<KeCheng>("SELECT top 8 * FROM kecheng where zhiding=0 and shifouxianshi=1 and kechengleixing='科学探秘' order by id desc", mystr);

            List<KeCheng> kc5 = Utils.SqlstrToModels<KeCheng>("SELECT top 1 * FROM kecheng where zhiding=1 and kechengleixing='文史天地' and shifouxianshi=1 order by id desc", mystr);
            List<KeCheng> kc6 = Utils.SqlstrToModels<KeCheng>("SELECT top 8 * FROM kecheng where zhiding=0 and shifouxianshi=1 and kechengleixing='文史天地' order by id desc", mystr);

            kechengs[0] = kc1;
            kechengs[1] = kc2;
            kechengs[2] = kc3;
            kechengs[3] = kc4;
            kechengs[4] = kc5;
            kechengs[5] = kc6;
            ViewData.Model = kechengs;
            return View();
        }

        public ActionResult NewList()
        {
            List<XinWen>[] xws = new List<XinWen>[2];
            List<XinWen> xw1 = Utils.SqlstrToModels<XinWen>("select * from xinwen where shifouxianshi=1 order by shijian desc", mystr);
            xws[0] = xw1;
            xws[1] = zuixinxinwens;
            ViewData.Model = xws;
            ViewBag.kechengs = zuixinkechengs;
            return View();
        }

        public ActionResult New()
        {
            string id = Request.QueryString["id"];
            string canshu = Request.QueryString["cs"];
            if(canshu=="b")
            {
                XinWen xw = new XinWen();
                xw.shilihua(id);
                xw.jiayi();
                ViewData.Model = xw;

            }
            else
            {
                XinWen xw = new XinWen();
                xw.shilihua(id);
                ViewData.Model = xw;
            }
            ViewBag.xinwens = zuixinxinwens;
            ViewBag.kechengs = zuixinkechengs;
            return View();
        }


        public ActionResult KeChengList()
        {
            List<KeCheng> kechengs = Utils.SqlstrToModels<KeCheng>("select * from kecheng where shifouxianshi=1", mystr);
            ViewData.Model = kechengs;
            return View();
        }

        public ActionResult KeCheng()
        {
            string id = Request.QueryString["id"];
            string canshu = Request.QueryString["cs"];
            if (canshu == "b")
            {
                KeCheng kc = new KeCheng();
                kc.shilihua(id);
                kc.jiayi();
                ViewData.Model = kc;

            }
            else
            {
                KeCheng kc = new KeCheng();
                kc.shilihua(id);
                ViewData.Model = kc;
            }
            
            ViewBag.kechengs = zuixinkechengs;
            return View();
        }
    }
}