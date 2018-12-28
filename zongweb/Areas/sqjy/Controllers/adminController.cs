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
    public class adminController : Controller
    {
        public static string mystr = ConfigurationManager.AppSettings["ConnectionString2"].ToString();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string un,string mm)
        {
            Admin admin = new Admin();
            if(admin.Login(un,mm))
            {
                Session["ADMIN"] = admin;
                return Content("true");
            }
            else
            {
                return Content("false");
            }
        }

        public ActionResult xinwenguanli()
        {
            if (Session["ADMIN"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                List<XinWen> xinwens = Utils.SqlstrToModels<XinWen>("select * from xinwen ORDER BY shijian desc", mystr);
                ViewData.Model = xinwens;
                return View();
            }
        }

        public ActionResult tianjiaxinwen()
        {
            if (Session["ADMIN"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult tianjiaxinwen(string xwbt, string xwlx, string xwlr, string xwjj, string fmtp, string fssj)
        {
            int time = Utils.ConvertDateTimeInt(DateTime.Now);
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "INSERT INTO xinwen (biaoti, shijian,zuozhe,liulanliang,leirong,leixing,jianjie,fenmiantupian,shifouxianshi,shijian2) VALUES ('" + xwbt + "'," + time + ",'管理员',0,'" + xwlr + "','" + xwlx + "','" + xwjj + "','" + fmtp + "',0,'" + fssj + "')";
            cmd.ExecuteNonQuery();

            return Content("true");

        }
    }
}