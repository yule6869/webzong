using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using chepingxitong;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeiXinKaifa;

namespace webzong2.Areas.cheping.Controllers
{
    public class xueshengController : Controller
    {
        public ActionResult rukou()
        {
            if(Session["XUESHENG"]==null)
            {
             
                XueSheng xs = new XueSheng();
                xs.shilihua("2019010001");
                Session["XUESHENG"] = xs;
                return RedirectToAction("shouye2");
            }
            else
            {
                return RedirectToAction("shouye2");
            }
        }

        public ActionResult shouye2()
        {
            if (Session["XUESHENG"] == null)
            {
                return RedirectToAction("/rukou");
            }
            else
            {
                
                XueSheng xuesheng = (XueSheng)Session["XUESHENG"];
                ViewData.Model = xuesheng;
                string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
                SqlConnection con = new SqlConnection(mystr);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "select count(*) from timu where zhuangtai=2";
                ViewBag.zongtiliang = (int)cmd.ExecuteScalar();
                con.Close();
                cmd.Dispose();
                return View();
            }
        }

        public ActionResult Index()
        {
            if (Request["code"] == null)
            {
                string url = OAuth.GetAuthUrl(WxBaseConfig.APPID, "http://sqjy.qxntvu.com/cheping/xuesheng/Index", "A", AuthType.snsapi_userinfo);
                return Redirect(url);
            }
            else
            {
                string code = Request.QueryString["code"].ToString();
                OAuthToken token = OAuth.GetAuthToken(WxBaseConfig.APPID, WxBaseConfig.APPSECRET, code);
                UserInfo userInfo = OAuth.GetUserInfo(token.access_token, token.openid);
                
                    if (!string.IsNullOrEmpty(userInfo.openid))
                    {
                        XueSheng xuesheng = new XueSheng();
                        if (xuesheng.login(userInfo))
                        {
                            Session["XUESHENG"] = xuesheng;
                            return RedirectToAction("/shouye");
                        }
                        else
                        {
                            Session["USERINFO"] = userInfo;
                            return RedirectToAction("/bangding");
                        }
                    }
                    else
                    {
                        return Redirect(OAuth.GetAuthUrl(WxBaseConfig.APPID, "http://sqjy.qxntvu.com/cheping/xuesheng/Index", "A", AuthType.snsapi_userinfo));
                    }
                


            }

        }


        public ActionResult bangding()
        {
            return View();
        }

        [HttpPost]
        public ActionResult bangding(string un, string sj)
        {

            UserInfo us = (UserInfo)Session["USERINFO"];
            XueSheng xuesheng = new XueSheng();
            if (xuesheng.login(un, sj, us))
            {
                Session["XUESHENG"] = xuesheng;
                return Content("true");
            }
            else
            {
                return Content("false");
            }

        }

        public ActionResult shouye()
        {
            if (Session["XUESHENG"] == null)
            {
                return RedirectToAction("/rukou");
            }
            else
            {
                XueSheng xuesheng = (XueSheng)Session["XUESHENG"];
                ViewData.Model = xuesheng;
                string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
                SqlConnection con = new SqlConnection(mystr);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "select count(*) from timu where zhuangtai=2";
                ViewBag.zongtiliang = (int)cmd.ExecuteScalar();
                con.Close();
                cmd.Dispose();


                return View();
            }
        }


        /// <summary>
        /// 打开试卷页面
        /// </summary>
        /// <returns></returns>
        public ActionResult shijuan()
        {
            if (Session["XUESHENG"] == null)
            {
                return RedirectToAction("/Index");
            }
            else
            {
                string shijuanhao = Request.QueryString["sjh"];
                ShiJuan shijuan = new ShiJuan(shijuanhao);
                shijuan.getsjtms();
                string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
                SqlConnection con = new SqlConnection(mystr);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "update shijuan set zhuangtai='" + ShiJuanZhuangTai.继续答题 + "' where id=" + shijuanhao;
                cmd.ExecuteNonQuery();
                con.Close();
                cmd.Dispose();
                ViewData.Model = shijuan;
                return View();
            }

        }

        /// <summary>
        /// 保存客户端每5分钟传过来的答卷
        /// </summary>
        /// <returns></returns>
        public ActionResult baocun()
        {
            if (Session["XUESHENG"] == null)
            {
                return Content("false");
            }
            else
            {
                string shijuanhao = Request.Form["sjh"];
                ShiJuan sj = new ShiJuan(shijuanhao);
                if (sj._zhuangtai == "继续答题")
                {
                    string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
                    SqlConnection con = new SqlConnection(mystr);
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    con.Open();


                    string answers = Request.Form["das"];
                    JArray jsonObj = (JArray)JsonConvert.DeserializeObject(answers);
                    for (int i = 0; i < jsonObj.Count; i++)
                    {
                        cmd.CommandText = "update sjtm set xuanxiangid=" + jsonObj[i]["daan"] + " where shijuanid=" + shijuanhao + " and timuid=" + jsonObj[i]["id"];
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                    cmd.Dispose();
                    return Content("true");
                }
                else
                {
                    return Content("false");
                }
            }
        }

        /// <summary>
        /// 接收学生提交的试卷
        /// </summary>
        /// <returns></returns>
        public ActionResult jiaojuan()
        {
            if (Session["XUESHENG"] == null)
            {
                return Content("false");
            }
            else
            {


                string shijuanhao = Request.Form["sjh"];
                string answers = Request.Form["das"];
                JArray jsonObj = (JArray)JsonConvert.DeserializeObject(answers);
                ShiJuan sj = new ShiJuan(shijuanhao);
                if (sj._zhuangtai == "继续答题")
                {
                    string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
                    SqlConnection con = new SqlConnection(mystr);
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    con.Open();
                    for (int i = 0; i < jsonObj.Count; i++)
                    {
                        cmd.CommandText = "update sjtm set xuanxiangid=" + jsonObj[i]["daan"] + " where shijuanid=" + shijuanhao + " and timuid=" + jsonObj[i]["id"];
                        cmd.ExecuteNonQuery();
                    }
                    cmd.CommandText = "update shijuan set zhuangtai='"+ShiJuanZhuangTai.已提交+"',tijiaoshijian=" + Utils.ConvertDateTimeInt(DateTime.Now) + " where id=" + shijuanhao;
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "update kaoshi set tijiaoshu=tijiaoshu+1 where id=" + sj._kaoshiid;
                    cmd.ExecuteNonQuery();
                    con.Close();
                    cmd.Dispose();
                    return Content("true");
                }
                else
                {
                    return Content("false");
                }
            }
        }



        public ActionResult chakanshijuan()
        {
            if (Session["XUESHENG"] == null)
            {
                return Content("false");
            }
            else
            {
                string shijuanhao = Request.QueryString["sjh"];
                ShiJuan shijuan = new ShiJuan(shijuanhao);
                shijuan.getsjtms();
                ViewData.Model = shijuan;
                ViewBag.zhuangtai = shijuan._zhuangtai;
                return View();
            }
        }

        public ActionResult paiming()
        {
            if (Session["XUESHENG"] == null)
            {
                return Content("false");
            }
            else
            {
                string kaoshiid = Request.QueryString["ksid"];
                string mingci = Request.QueryString["mc"];
                KaoShi ks = new KaoShi(kaoshiid);
                ks.GetShiJuanIds();
                ViewData.Model = ks;
                ViewBag.mingci = mingci.Trim();
                return View();
            }
        }





























        public ActionResult xueshengshouye()
        {
            if(Session["XUESHENG"]==null)
            {
                return RedirectToAction("rukou");
            }
            else
            {
                XueSheng xs = (XueSheng)Session["XUESHENG"];
                JavaScriptSerializer jss = new JavaScriptSerializer();
                ViewBag.xs = jss.Serialize(xs);
                return View();
            }
        }

        public JsonResult getxuesheng()
        {
            JsonResult b = new JsonResult();
            if (Session["XUESHENG"]==null)
            {
                b.Data = new { reslut = "false" };
                return b;
            }
            else
            {
                XueSheng xs = (XueSheng)Session["XUESHENG"];
                b.Data = new { reslut = "true", xues = xs };
                return b;
            }
        }

        /// <summary>
        /// 我的考试页面
        /// </summary>
        /// <returns></returns>
        public ActionResult wodekaoshi()
        {
            if (Session["XUESHENG"] == null)
            {
                return Content("false");
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 获取试卷s
        /// </summary>
        /// <returns></returns>
        public JsonResult getshijuans()
        {
            JsonResult b = new JsonResult();
            if (Session["XUESHENG"] == null)
            {
                b.Data = new { reslut = "false" };
                return b;
            }
            else
            {
                XueSheng xs = (XueSheng)Session["XUESHENG"];
                List<ShiJuan> shijuans = xs.GetShiJuans();
                b.Data = new { reslut = "true", sjs = shijuans };
                return b;
            }
        }


        /// <summary>
        /// 考试页面
        /// </summary>
        /// <returns></returns>
        public ActionResult kaoshi()
        {
            if (Session["XUESHENG"] == null)
            {
                return Content("false");
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 通过试卷id获取试卷
        /// </summary>
        /// <param name="id">试卷id</param>
        /// <returns></returns>
        public JsonResult getshijuan(string id)
        {
            JsonResult b = new JsonResult();
            if (Session["XUESHENG"] == null)
            {
                b.Data = new { reslut = "false" };
                return b;
            }
            else
            {
                XueSheng xs = (XueSheng)Session["XUESHENG"];
                ShiJuan shijuan = xs.GetShiJuan(id);
                b.Data = new { reslut = "true", sj = shijuan };
                return b;
            }
        }

        //public ActionResult ceshi()
        //{
        //    Regex rg = new Regex("style=\"(.*?)\"", RegexOptions.IgnoreCase);
        //    string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
        //    SqlConnection con = new SqlConnection(mystr);
        //    SqlConnection con2 = new SqlConnection(mystr);
        //    SqlCommand cmd = new SqlCommand();
        //    SqlCommand cmd2 = new SqlCommand();
        //    cmd.CommandText = "select * from xuanxiang";
        //    cmd2.Connection = con2;
        //    cmd.Connection = con;
        //    con.Open();
        //    con2.Open();
        //    SqlDataReader myDr = cmd.ExecuteReader();
        //    while (myDr.Read())
        //    {
        //        string str = myDr["xuanxiang"].ToString().Trim();
        //        var m = rg.Match(str);
        //        if (m.Success)
        //        {
        //            str = str.Replace(m.Groups[0].Value, "style=\"max-width:90%;\"");
        //            cmd2.CommandText = "update xuanxiang set xuanxiang=N'" + str + "' where id=" + myDr["id"].ToString();
        //            cmd2.ExecuteNonQuery();

        //        }
                
        //    }
        //    myDr.Close();
        //    cmd.Dispose();
        //    cmd2.Dispose();
        //    con.Close();
        //    con2.Close();
        //    return Content("true");
 
        //}
    }
}