using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pachong.Models;
using chepingxitong;
using WeiXinKaifa;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using pachong.Models;
using OfficeOpenXml;
using System.IO;
using System.Net;

namespace webzong2.Areas.cheping.Controllers
{
    public class jiaoshiController : Controller
    {

        public ActionResult Index()
        {
            if (Request["code"] == null)
            {
                string url = OAuth.GetAuthUrl(WxBaseConfig.APPID, "http://g224171f50.iok.la/cheping/jiaoshi/Index", "A", AuthType.snsapi_userinfo);
                return Redirect(url);
            }
            else
            {
                string code = Request.QueryString["code"].ToString();
                OAuthToken token = OAuth.GetAuthToken(WxBaseConfig.APPID, WxBaseConfig.APPSECRET, code);
                UserInfo userInfo = OAuth.GetUserInfo(token.access_token, token.openid);

                if (!string.IsNullOrEmpty(userInfo.openid))
                {
                    JiaoShi js = new JiaoShi();
                    if (js.login(userInfo))
                    {
                        Session["JIAOSHI"] = js;
                        return RedirectToAction("/jiaoshishouye");
                    }
                    else
                    {
                        Session["USERINFO"] = userInfo;
                        return RedirectToAction("/Login");
                    }
                }
                else
                {
                    return Redirect(OAuth.GetAuthUrl(WxBaseConfig.APPID, "http://g224171f50.iok.la/cheping/jiaoshi/Index", "A", AuthType.snsapi_userinfo));
                }
            }
        }

        // GET: cheping/jiaoshi
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string un, string sj)
        {
            if (Session["USERINFO"] == null)
            {
                JiaoShi jiaoshi = new JiaoShi();
                if (jiaoshi.login(un, sj))
                {
                    Session["JIAOSHI"] = jiaoshi;
                    return Content("true");
                }
                else
                {
                    return Content("false");
                }
            }
            else
            {
                UserInfo us = (UserInfo)Session["USERINFO"];
                JiaoShi js = new JiaoShi();
                if (js.login(un, sj, us))
                {
                    Session["JIAOSHI"] = js;
                    return Content("true");
                }
                else
                {
                    return Content("false");
                }
            }
        }
        /// <summary>
        /// 教师端入口
        /// </summary>
        /// <returns></returns>
        public ActionResult rukou()
        {
            if (Session["JIAOSHI"] == null)
            {
                JiaoShi jiaoshi = new JiaoShi();
                jiaoshi.login("yule", "17384196869");
                Session["JIAOSHI"] = jiaoshi;
                return RedirectToAction("jiaoshishouye");
            }
            else
            {
                return RedirectToAction("jiaoshishouye");
            }
        }
        /// <summary>
        /// 教师手机端首页
        /// </summary>
        /// <returns></returns>
        public ActionResult jiaoshishouye()
        {
            if (Session["JIAOSHI"] == null)
            {

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// 题库管理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult tikuguanli()
        {
            if (Session["JIAOSHI"] == null)
            {

                return Content("false");
            }
            else
            {
                KeCheng kecheng = new KeCheng();
                kecheng.shilihua("1");
                JavaScriptSerializer jss = new JavaScriptSerializer();
                ViewBag.zj = jss.Serialize(kecheng._zhangs);
                return View();
            }

        }
        //public ActionResult ruku()
        //{

        //    string str = PaChong.ReadJsonFile("D:\\789.txt");
        //    JObject jobj2 = JsonConvert.DeserializeObject<JObject>(str);
        //    List<Options> options = JsonConvert.DeserializeObject<List<Options>>(jobj2.GetValue("Option").ToString());
        //    List<Question> questions = JsonConvert.DeserializeObject<List<Question>>(jobj2.GetValue("Question").ToString());
        //    List<Topic> topics = JsonConvert.DeserializeObject<List<Topic>>(jobj2.GetValue("Topic").ToString());
        //    string[] abc = { "A", "B", "C", "D", "E" };
        //    foreach (Question item in questions)
        //    {
        //        string timu = paxuehuli.imgchuli(item.Title);
        //        List<XuanXiang> xuanxiangs = new List<XuanXiang>();
        //        foreach (Options item2 in item.GetOptions(options))
        //        {
        //            bool trr = true;
        //            if (item2.IsTrue == "0")
        //                trr = false;
        //            XuanXiang xx = new XuanXiang(item2.Option, "1", trr);
        //            xuanxiangs.Add(xx);
        //        }
        //        string jiexi = paxuehuli.imgchuli(item.Explain);

        //        if (!string.IsNullOrEmpty(item.Topic))
        //        {
        //            Topic tp = topics.Find(delegate (Topic p) { return p.Id == item.Topic.Trim(); });
        //            TiGan tg = new TiGan(paxuehuli.imgchuli(tp.Content));
        //            DanXuanTi danxuanti = new DanXuanTi(timu, tg._id, "453", jiexi, xuanxiangs);
        //        }
        //        else
        //        {
        //            DanXuanTi danxuanti = new DanXuanTi(timu, "", "453", jiexi, xuanxiangs);

        //        }

        //    }
        //    return Content("true");

        //}

        /// <summary>
        /// 返回题目数据s
        /// </summary>
        /// <param name="jieid">节</param>
        /// <param name="ye">页</param>
        /// <param name="sl">数量</param>
        /// <param name="cx">查询</param>
        /// <param name="zt">状态</param>
        /// <param name="sj">是否随机</param>
        /// <param name="ly">题目来源</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult gettimus(string jieid, string ye, string sl, string cx, string zt, string sj, string ly)
        {
            JsonResult b = new JsonResult();
            if (Session["JIAOSHI"] == null)
            {
                b.Data = new { reslut = "false" };
                return b;
            }
            else
            {
                List<DanXuanTi> timus = new List<DanXuanTi>();
                string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
                SqlConnection con = new SqlConnection(mystr);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();
                int z = 0;
                string sql = "select";
                string sql2 = "select";
                string sql3 = "select count(id) from timu where 1=1";
                if (sl != null)
                {
                    sql = sql + " top " + sl;
                    if (ye != null)
                    {
                        z = int.Parse(ye);
                        if (z > 1)
                        {
                            sql2 = sql2 + " top " + (z - 1) * int.Parse(sl);
                        }
                    }
                }
                sql = sql + " id from timu where 1=1";
                sql2 = sql2 + " id from timu where 1=1";

                if (jieid != null)
                {
                    sql = sql + " and jieid=" + jieid;
                    sql2 = sql2 + " and jieid=" + jieid;
                    sql3 = sql3 + " and jieid=" + jieid;
                }
                if (cx != null)
                {
                    sql = sql + " and timu like '%" + cx + "%'";
                    sql2 = sql2 + " and timu like '%" + cx + "%'";
                    sql3 = sql3 + " and timu like '%" + cx + "%'";
                }
                if (zt != null)
                {
                    sql = sql + " and zhuangtai=" + zt;
                    sql2 = sql2 + " and zhuangtai=" + zt;
                    sql3 = sql3 + " and zhuangtai=" + zt;
                }
                if(!string.IsNullOrEmpty(ly))
                {
                    switch(ly)
                    {
                        case "xuehuli":
                            sql = sql + " and laiyuan='雪狐狸'";
                            sql2 = sql2 + " and laiyuan='雪狐狸'";
                            sql3 = sql3 + " and laiyuan='雪狐狸'";
                            break;
                        case "zijian":
                            sql = sql + " and laiyuan='自建'";
                            sql2 = sql2 + " and laiyuan='自建'";
                            sql3 = sql3 + " and laiyuan='自建'";
                            break;

                    }
                }
                if (sj != null && sj == "1")
                {
                    sql = sql + " order by NEWID()";
                    sql2 = sql2 + " order by NEWID()";
                }
                if (z > 1)
                {
                    sql = sql + " and id not in (" + sql2 + ")";
                }



                cmd.CommandText = sql3;
                int zongtiaoshu = (int)cmd.ExecuteScalar();

                cmd.CommandText = sql;
                SqlDataReader mydr = cmd.ExecuteReader();
                while (mydr.Read())
                {
                    DanXuanTi tm = new DanXuanTi(mydr["id"].ToString().Trim());
                    timus.Add(tm);
                }
                mydr.Close();
                con.Close();
                cmd.Dispose();

                b.Data = new { reslut = "true", tms = timus, sl = timus.Count, ym = ye, zs = zongtiaoshu };
                return b;
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult updatetimu()
        {
            if (Session["JIAOSHI"] == null)
            {

                return Content("false");
            }
            else
            {
                string leibie = Request.Form["lb"];
                string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
                SqlConnection con = new SqlConnection(mystr);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();
                switch (leibie)
                {
                    case "daan":
                        string timuid = Request.Form["tmid"];
                        string xuanxiangid = Request.Form["xxid"];
                        cmd.CommandText = "update xuanxiang set IsTrue=0 where timuid=" + timuid;
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "update xuanxiang set IsTrue=1 where id=" + xuanxiangid;
                        cmd.ExecuteNonQuery();
                        break;
                    case "zhangjie":
                        string timuid2 = Request.Form["tmid"];
                        string jieid = Request.Form["jieid"];
                        TiMu timu = new TiMu(timuid2);
                        timu.zhuanjie(jieid);
                        break;
                    case "tigan":
                        string tiganid = Request.Form["tgid"];
                        string tigan = Request.Form["tg"];
                        cmd.CommandText = "update tigan set tigan=N'" + tigan + "' where id=" + tiganid;
                        cmd.ExecuteNonQuery();
                        break;
                    case "timu":
                        string timuid3 = Request.Form["tmid"];
                        string timu2 = Request.Form["tm"];
                        cmd.CommandText = "update timu set timu=N'" + timu2 + "' where id=" + timuid3;
                        cmd.ExecuteNonQuery();
                        break;
                    case "jiexi":
                        string timuid4 = Request.Form["tmid"];
                        string jiexi = Request.Form["jx"];
                        cmd.CommandText = "update timu set jiexi=N'" + jiexi + "' where id=" + timuid4;
                        cmd.ExecuteNonQuery();
                        break;
                    case "xuanxiang":
                        string xuanxiangid2 = Request.Form["xxid"];
                        string xuanxiang = Request.Form["xx"];
                        cmd.CommandText = "update xuanxiang set xuanxiang=N'" + xuanxiang + "' where id=" + xuanxiangid2;
                        cmd.ExecuteNonQuery();
                        break;
                }
                con.Close();
                cmd.Dispose();
                return Content("true");
            }
        }
        public ActionResult ceshi(string zhangming, string zhangid)
        {
            if (zhangming != null)
            {
                //paxuehuli pa = new paxuehuli();
                //pa.Login();
                //List<pachong.Models.Jie> jies = pa.GetJies("59544");
                //return Content("true");
                paxuehuli pa = new paxuehuli();
                if (Session["paxuehuli"] == null)
                {

                    pa.Login();
                    Session["paxuehuli"] = pa;
                }
                else
                {
                    pa = (paxuehuli)Session["paxuehuli"];
                }

                chepingxitong.Zhang zhang = new chepingxitong.Zhang(zhangming, "1");//章的名称
                List<pachong.Models.Jie> jies1 = pa.GetJies(zhangid);//章的id
                foreach (pachong.Models.Jie item2 in jies1)
                {
                    chepingxitong.Jie jie = new chepingxitong.Jie(item2.Name, zhang._id);
                    string timuJson = pa.GetTiMus(item2.Id);
                    if (timuJson != "")
                    {
                        JObject jobj2 = JsonConvert.DeserializeObject<JObject>(timuJson);
                        List<Options> options = JsonConvert.DeserializeObject<List<Options>>(jobj2.GetValue("Option").ToString());
                        List<Question> questions = JsonConvert.DeserializeObject<List<Question>>(jobj2.GetValue("Question").ToString());
                        List<Topic> topics = JsonConvert.DeserializeObject<List<Topic>>(jobj2.GetValue("Topic").ToString());
                        foreach (Question item3 in questions)
                        {
                            string timu = paxuehuli.imgchuli(item3.Title);
                            List<XuanXiang> xuanxiangs = new List<XuanXiang>();
                            foreach (Options item4 in item3.GetOptions(options))
                            {
                                bool trr = true;
                                if (item4.IsTrue == "0")
                                    trr = false;
                                XuanXiang xx = new XuanXiang(item4.Option, "1", trr);
                                xuanxiangs.Add(xx);
                            }
                            string jiexi = paxuehuli.imgchuli(item3.Explain);

                            if (!string.IsNullOrEmpty(item3.Topic))
                            {
                                Topic tp = topics.Find(delegate (Topic p) { return p.Id == item3.Topic.Trim(); });
                                TiGan tg = new TiGan(paxuehuli.imgchuli(tp.Content));
                                DanXuanTi danxuanti = new DanXuanTi(timu, tg._id, jie._id, jiexi, xuanxiangs);
                            }
                            else
                            {
                                DanXuanTi danxuanti = new DanXuanTi(timu, "", jie._id, jiexi, xuanxiangs);

                            }

                        }
                    }
                }
                return Content(zhangming + "," + zhangid + ":" + "true");
            }
            else
            {
                return View();
            }


        }

        public ActionResult ceshi2()
        {
            WebResponse response = null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.baid334u.com/dfsdkf/defc");
            request.Timeout = 3000;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
            }
            finally
            {
                if (response != null) response.Close();
            }
            return Content("fsdf");
        }
        /// <summary>
        /// 更新题量
        /// </summary>
        /// <returns></returns>
        public ActionResult genxintiliang()
        {
            KeCheng kecheng = new KeCheng();
            kecheng.shilihua("1");
            foreach (chepingxitong.Zhang item in kecheng._zhangs)
            {
                item.genxintiliang();
            }
            return Content("true");

        }


        /// <summary>
        /// 题库效验页面
        /// </summary>
        /// <returns></returns>
        public ActionResult tikuxiaoyan()
        {
            if (Session["JIAOSHI"] == null)
            {

                return Content("false");
            }
            else
            {
                KeCheng kecheng = new KeCheng();
                kecheng.shilihua("1");
                JavaScriptSerializer jss = new JavaScriptSerializer();
                ViewBag.zj = jss.Serialize(kecheng._zhangs);
                return View();
            }
        }


        /// <summary>
        /// 效验通过
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult xiaoyantongguo(string timuid)
        {
            if (Session["JIAOSHI"] == null)
            {

                return Content("false");
            }
            else
            {
                TiMu timu = new TiMu(timuid);
                chepingxitong.Jie jie = new chepingxitong.Jie(timu._jieid);
                string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
                SqlConnection con = new SqlConnection(mystr);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "update timu set zhuangtai=2 where id=" + timuid;
                cmd.ExecuteNonQuery();

                cmd.CommandText = "update jie set tiliang1=tiliang1-1,tiliang2=tiliang2+1 where id=" + jie._id;
                cmd.ExecuteNonQuery();

                cmd.CommandText = "update zhang set tiliang1=tiliang1-1,tiliang2=tiliang2+1 where id=" + jie._zhangid;
                cmd.ExecuteNonQuery();
                con.Close();
                cmd.Dispose();
                return Content("true");
            }
        }


        /// <summary>
        /// 考试管理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult kaoshiguanli()
        {
            if (Session["JIAOSHI"] == null)
            {

                return Content("false");
            }
            else
            {

                return View();
            }
        }

        /// <summary>
        /// 发布考试页面
        /// </summary>
        /// <returns></returns>
        public ActionResult fabukaoshi()
        {
            if (Session["JIAOSHI"] == null)
            {

                return Content("false");
            }
            else
            {
                JiaoShi jiaoshi = (JiaoShi)Session["JIAOSHI"];
                KeCheng kc = new KeCheng();
                kc.shilihua("1");
                JavaScriptSerializer jss = new JavaScriptSerializer();
                ViewBag.zj = jss.Serialize(kc._zhangs);
                ViewBag.bjs = jiaoshi.GetBanJis("1");
                return View();
            }
        }


        /// <summary>
        /// 发布考试动作
        /// </summary>
        /// <param name="ksm">考试名称</param>
        /// <param name="tmids">题目的id组</param>
        /// <param name="bhs">班级的id组</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult fabukaoshiaction(string ksm,string[] tmids,string[] bhs)
        {
            JsonResult b = new JsonResult();
            if (Session["JIAOSHI"] == null)
            {
                b.Data = new { reslut = "false" };
                return b;
            }
            else
            {
                JiaoShi jiaoshi = (JiaoShi)Session["JIAOSHI"];

                KaoShi kaosh = new KaoShi(ksm, jiaoshi._id, "1", tmids, bhs);

                b.Data = new { reslut = "true" };
                return b;
            }
        }

        /// <summary>
        /// 获取考试列表数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult getkaoshis()
        {
            JsonResult b = new JsonResult();
            if (Session["JIAOSHI"] == null)
            {
                b.Data = new { reslut = "false" };
                return b;
            }
            else
            {
                KeCheng kc = new KeCheng("1");
                List<KaoShi> ks = kc.GetKaoShis();
                b.Data = new { reslut = "true", kss = ks };
                return b;
            }
        }


        /// <summary>
        /// 发布考试
        /// </summary>
        /// <param name="ksid">考试的id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult fabukaoshi(string ksid)
        {
            JsonResult b = new JsonResult();
            if (Session["JIAOSHI"] == null)
            {
                b.Data = new { reslut = "false" };
                return b;
            }
            else
            {
                JiaoShi jiaoshi = (JiaoShi)Session["JIAOSHI"];
                int fabushijian = Utils.ConvertDateTimeInt(DateTime.Now);
                jiaoshi.fabukaoshi(ksid,fabushijian);
                b.Data = new { reslut = "true", fbsj = fabushijian };
                return b;
            }
        }


        /// <summary>
        /// 删除考试
        /// </summary>
        /// <param name="ksid">考试的id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult shanchukaoshi(string ksid)
        {
            JsonResult b = new JsonResult();
            if (Session["JIAOSHI"] == null)
            {
                b.Data = new { reslut = "false" };
                return b;
            }
            else
            {
                JiaoShi jiaoshi = (JiaoShi)Session["JIAOSHI"];
                
                jiaoshi.shanchukaoshi(ksid);
                b.Data = new { reslut = "true"};
                return b;
            }
        }

        /// <summary>
        /// 手机端查看交卷情况页
        /// </summary>
        /// <returns></returns>
        public ActionResult chakanqingkuang()
        {
            if (Session["JIAOSHI"] == null)
            {
                return Content("false");
            }
            else
            {

                return View();
            }
        }


        /// <summary>
        /// 手机端获取考试试卷s
        /// </summary>
        /// <param name="id">考试id</param>
        /// <returns>试卷s</returns>
        [HttpPost]
        public JsonResult getshijuans(string id)
        {
            JsonResult b = new JsonResult();
            if (Session["JIAOSHI"] == null)
            {
                b.Data = new { reslut = "false" };
                return b;
            }
            else
            {
                List<object> lists = new List<object>();
                string cs = Request.QueryString["cs"];
                KaoShi ks = new KaoShi(id);
                ks.GetShiJuanIds();
                if (cs == "min")
                {
                    foreach (string item in ks._shijuanids)
                    {
                        ShiJuan sj = new ShiJuan(item);
                        XueSheng xs = new XueSheng();
                        xs.shilihua(sj._xueshengid);
                        lists.Add(new
                        {
                            sjid = sj._id,
                            sjzt = sj._zhuangtai,
                            xsxm = xs._xingming,
                            xstx = xs._touxiang,
                            zs = sj._tiliang,
                            ztl = sj._yizuoti,
                            wcd = Math.Round((double)sj._yizuoti / sj._tiliang, 4)
                        });
                    }
                    b.Data = new { reslut = "true", shujus = lists };
                }
                else
                {
                    List<KaoShiTiMuTongJi> timus = new List<KaoShiTiMuTongJi>();
                    foreach (string item2 in ks._timuids)
                    {
                        KaoShiTiMuTongJi tj = new KaoShiTiMuTongJi(ks._id,item2);
                        timus.Add(tj);
                    }
                    foreach (string item in ks._shijuanids)
                    {
                        ShiJuan sj = new ShiJuan(item);
                        XueSheng xs = new XueSheng();
                        xs.shilihua(sj._xueshengid);
                        lists.Add(new
                        {
                            sjid = sj._id,
                            sjzt = sj._zhuangtai,
                            xsxm = xs._xingming,
                            xstx = xs._touxiang,
                            zs = sj._tiliang,
                            mc = sj._mingci,
                            zql = Math.Round((double)sj._zhengqueti / sj._tiliang, 4)

                    });
                    }
                    b.Data = new { reslut = "true", shujus = lists, timutongjis = timus };

                }

                return b;
            }
        }


        /// <summary>
        /// 收卷
        /// </summary>
        /// <param name="ksid">需要收取的考试id</param>
        /// <returns>收卷成功的考试</returns>
        [HttpPost]
        public JsonResult shoujuan(string ksid)
        {
            JsonResult b = new JsonResult();
            if (Session["JIAOSHI"] == null)
            {
                b.Data = new { reslut = "false" };
                return b;
            }
            else
            {
                KaoShi kaoshi = new KaoShi(ksid);
                kaoshi.GetShiJuanIds();
                kaoshi.shoujuan();

                b.Data = new { reslut = "true", ks = kaoshi };
                return b;
            }
        }

        /// <summary>
        /// 查看考试收卷后的情况
        /// </summary>
        /// <returns></returns>
        public ActionResult ckksqk()
        {
            if (Session["JIAOSHI"] == null)
            {
                return Content("false");
            }
            else
            {

                return View();
            }
        }







        public ActionResult ceshi3()
        {
            return View();
        }

        //public ActionResult guoqu()
        //{
        //    string mystr = ConfigurationManager.AppSettings["ConnectionString3"].ToString();
        //    SqlConnection con = new SqlConnection(mystr);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = con;
        //    con.Open();
        //    cmd.CommandText = "select * from tiku";
        //    SqlDataReader mydr = cmd.ExecuteReader();
        //    while(mydr.Read())
        //    {
        //        chepingxitong.Zhang zhang = new chepingxitong.Zhang();
        //        zhang.zinenggz(mydr["suozaizhang"].ToString().Trim());

        //        chepingxitong.Jie jie = new chepingxitong.Jie();
        //        jie.zinenggz(zhang._id, mydr["suozaijie"].ToString().Trim());

        //        chepingxitong.TiMu timu = new chepingxitong.TiMu();
        //        if(Utils.chuli(mydr["tupian"])!="")
        //        {
        //            timu._timu = mydr["timu"].ToString() + "<img src='" + Utils.chuli(mydr["tupian"]) + "' style='width:80%;' />";
        //        }
        //        else
        //        {
        //            timu._timu = mydr["timu"].ToString();
        //        }
        //        timu._leibie = "单选题";
        //        timu._jiexi = Utils.chuli(mydr["jiexi"]);
        //        timu._jieid = jie._id;
        //        timu._laiyuan = "自建";
        //        timu._zhuangtai = mydr["zhuangtai"].ToString().Trim();
        //        timu.daoru();

        //        if (Utils.chuli(mydr["tiganid"])!="")
        //        {
        //            TiGan tigan = new TiGan(Utils.chuli(mydr["tiganid"]));
        //            timu.charutigan(tigan._id);
        //        }

        //        string daan = mydr["daan"].ToString().Trim();
        //        bool z = false;

        //        if (daan == "A")
        //            z = true;
        //        XuanXiang xx1 = new XuanXiang(mydr["A"].ToString().Trim(), timu._id, z);
        //        xx1.daorushujuku();
        //        z = false;

        //        if (daan == "B")
        //            z = true;
        //        XuanXiang xx2 = new XuanXiang(mydr["B"].ToString().Trim(), timu._id, z);
        //        xx2.daorushujuku();
        //        z = false;

        //        if (daan == "C")
        //            z = true;
        //        XuanXiang xx3 = new XuanXiang(mydr["C"].ToString().Trim(), timu._id, z);
        //        xx3.daorushujuku();
        //        z = false;

        //        if (daan == "D")
        //            z = true;
        //        XuanXiang xx4 = new XuanXiang(mydr["D"].ToString().Trim(), timu._id, z);
        //        xx4.daorushujuku();
        //        z = false;

        //        if (daan == "E")
        //            z = true;
        //        XuanXiang xx5 = new XuanXiang(mydr["E"].ToString().Trim(), timu._id, z);
        //        xx5.daorushujuku();
        //        z = false;

        //    }
        //    con.Close();
        //    cmd.Dispose();
        //    return Content("true");
        //}

        //public ActionResult dd()
        //{
        //    string mystr = ConfigurationManager.AppSettings["ConnectionString3"].ToString();
        //    SqlConnection con = new SqlConnection(mystr);
        //    SqlCommand cmd = new SqlCommand();
        //    cmd.Connection = con;
        //    con.Open();
        //    cmd.CommandText = "select * from tigan";
        //    SqlDataReader mydr = cmd.ExecuteReader();
        //    while (mydr.Read())
        //    {
        //        TiGan tg = new TiGan(Utils.chuli(mydr["id"]));
        //        tg._tigan = mydr["tigan"].ToString().ToString();
        //        tg.genxin();
        //    }
        //    mydr.Close();
        //    con.Close();
        //    cmd.Dispose();
        //    return Content("true");
        //}
    }
}