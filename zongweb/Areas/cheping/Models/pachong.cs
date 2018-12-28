using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using WeiXinKaifa;
using chepingxitong;
using System.Collections;

namespace pachong.Models
{
    public class PaChong
    {
        public CookieContainer _cookie { get; set; }
        public PaChong()
        {
            _cookie = new CookieContainer();
        }

        /// <summary>
        /// 获取字符串中img的url集合
        /// </summary>
        /// <param name="content">字符串</param>
        /// <returns></returns>
        public static List<string> getImgUrl(string content)
        {
            Regex rg = new Regex("src=\"([^\"]+)\"", RegexOptions.IgnoreCase);
            var m = rg.Match(content);
            List<string> imgUrl = new List<string>();
            while (m.Success)
            {
                imgUrl.Add(m.Groups[1].Value); //这里就是图片路径                
                m = m.NextMatch();
            }
            return imgUrl;
        }

        /// <summary>
        /// 模拟登陆
        /// </summary>
        /// <param name="postParams">需要post的参数</param>
        /// <param name="url">登陆地址</param>
        /// <returns>服务器返回的string</returns>
        public string monilogin(Dictionary<string, string> postParams, string url)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                string postString = "";
                foreach (KeyValuePair<string, string> de in postParams)
                {
                    //把提交按钮中的中文字符转换成url格式,以防中文或空格等信息
                    postString += HttpUtility.UrlEncode(de.Key.ToString()) + "=" + HttpUtility.UrlEncode(de.Value.ToString()) + "&";
                }

                // 将提交的字符串数据转换成字节数组
                byte[] postData = Encoding.ASCII.GetBytes(postString);

                // 设置提交的相关参数
                request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                request.KeepAlive = false;
                request.ContentType = "application/x-www-form-urlencoded";
                request.CookieContainer = _cookie;
                request.ContentLength = postData.Length;
                request.AllowAutoRedirect = false;

                // 提交请求数据
                Stream outputStream = request.GetRequestStream();
                outputStream.Write(postData, 0, postData.Length);
                outputStream.Close();

                // 接收返回的页面
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                string srcString = reader.ReadToEnd();
                return srcString;
            }
            catch (WebException we)
            {
                string msg = we.Message;
                return msg;
            }
        }


        /// <summary>
        /// Get请求页面
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string HttpGet(string url)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";
                request.KeepAlive = false;
                request.CookieContainer = _cookie;

                // 接收返回的页面
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new System.IO.StreamReader(responseStream, Encoding.UTF8);
                string srcString = reader.ReadToEnd();
                return srcString;
            }
            catch (WebException we)
            {
                string msg = we.Message;
                return msg;
            }

        }

        public string HttpPost(Dictionary<string, string> postParams, string url)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                string postString = "";
                foreach (KeyValuePair<string, string> de in postParams)
                {
                    //把提交按钮中的中文字符转换成url格式,以防中文或空格等信息
                    postString += HttpUtility.UrlEncode(de.Key.ToString()) + "=" + HttpUtility.UrlEncode(de.Value.ToString()) + "&";
                }

                // 将提交的字符串数据转换成字节数组
                byte[] postData = Encoding.ASCII.GetBytes(postString);

                // 设置提交的相关参数
                request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                request.KeepAlive = true;
                request.ContentType = "application/x-www-form-urlencoded";
                request.CookieContainer = _cookie;
                request.ContentLength = postData.Length;
                request.AllowAutoRedirect = false;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; …) Gecko/20100101 Firefox/64.0";

                // 提交请求数据
                Stream outputStream = request.GetRequestStream();
                outputStream.Write(postData, 0, postData.Length);
                outputStream.Close();

                // 接收返回的页面
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                string srcString = reader.ReadToEnd();
                return srcString;
            }
            catch (WebException we)
            {
                string msg = we.Message;
                return msg;
            }
        }


        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Stream HttpGetImage(string url)
        {
            HttpWebResponse resp;
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.CookieContainer = _cookie;
            req.Timeout = 150000;
            resp = (HttpWebResponse)req.GetResponse();
            return resp.GetResponseStream();
        }

        /// <summary>
        /// 读取txt中的json
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns>json字符串</returns>
        public static string ReadJsonFile(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.UTF8);
            String line;
            string jsonobj = "";
            while ((line = sr.ReadLine()) != null)
            {
                jsonobj = jsonobj + line.ToString();
            }
            sr.Close();
            return jsonobj;
        }


        /// <summary>
        /// 将json数据写入text
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">写入的内容</param>
        public static void WriteJsonToFile(string path, string content)
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(content);
                sw.Close();
            }
        }

        


    }

    public class paxuehuli : PaChong
    {
        public static string tel = "17384196869";
        public static string pwd = "love6869";
        public static string subjectId = "74";//科目id
        public string guid;
        private static string TiMuUrl = "http://xuehuli.com/app/gettestmodel?urltimer=";
        /// <summary>
        /// 获取题目id的url
        /// </summary>
        private static string TiMuIdUrl = "http://xuehuli.com/app/gettid?urltimer=";
        /// <summary>
        /// 登陆地址
        /// </summary>
        private static string LoginUrl = "http://xuehuli.com/app/xhlpcuserlogin?urltimer=";

        /// <summary>
        /// 获取节的地址
        /// </summary>
        private static string JieUrl = "http://xuehuli.com/app/getcatalogue?urltimer=";

        public string GetJieUrl()
        {
            int shijian = Utils.ConvertDateTimeInt(DateTime.Now);
            return JieUrl + shijian;
        }
        public paxuehuli()
        {
            guid = "";
        }
        /// <summary>
        /// 获取登陆地址
        /// </summary>
        /// <returns></returns>
        public string GetLoginUrl()
        {
            int shijian = Utils.ConvertDateTimeInt(DateTime.Now);
            return LoginUrl + shijian;
        }

        public string GetTiMuIdUrl()
        {
            int shijian = Utils.ConvertDateTimeInt(DateTime.Now);
            return TiMuIdUrl + shijian;
        }
        public string GetTiMuUrl()
        {
            int shijian = Utils.ConvertDateTimeInt(DateTime.Now);
            return TiMuUrl + shijian;
        }
        /// <summary>
        /// 获取登陆需要的post的参数
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetLoginParams()
        {
            Dictionary<string, string> p = new Dictionary<string, string>();
            p.Add("pwd", pwd);
            p.Add("tel", tel);
            return p;
        }

        /// <summary>
        /// 登陆操作
        /// </summary>
        /// <returns></returns>
        public void Login()
        {
            string str = monilogin(GetLoginParams(), GetLoginUrl());
            JObject jobj = JsonConvert.DeserializeObject<JObject>(str);
            guid = jobj.GetValue("Guid").ToString();
        }


        /// <summary>
        /// 生成获取题目id的参数
        /// </summary>
        /// <param name="jieid">需要获取题目id的节的id(60050)</param>
        /// <returns>参数</returns>
        public Dictionary<string, string> GetTiMuIdParams(string jieid)
        {
            Dictionary<string, string> p = new Dictionary<string, string>();
            p.Add("cid", jieid);
            p.Add("guid", guid);
            p.Add("openid", "");
            p.Add("pwd", pwd);
            p.Add("subjectId", subjectId);
            p.Add("tel", tel);
            p.Add("type", "0");
            return p;
        }


        /// <summary>
        /// 获取那个节的题目ids
        /// </summary>
        /// <param name="jieid">需要获取题目id的节的id(60050)</param>
        /// <returns>ids</returns>
        public string GetTiMuId(string jieid)
        {
            return HttpPost(GetTiMuIdParams(jieid), GetTiMuIdUrl());
        }


        public Dictionary<string, string> GetTiMuParams(string jieid)
        {
            Dictionary<string, string> postParams3 = new Dictionary<string, string>();
            postParams3.Add("tid", GetTiMuId(jieid));
            postParams3.Add("guid", guid);
            postParams3.Add("openid", "");
            postParams3.Add("pwd", pwd);
            postParams3.Add("subjectId", subjectId);
            postParams3.Add("tel", tel);
            postParams3.Add("type", "0");
            return postParams3;
        }
        /// <summary>
        /// 获取题目s
        /// </summary>
        /// <param name="jieid">需要获取题目的节的id(60050)</param>
        /// <returns>题目json</returns>
        public string GetTiMus(string jieid)
        {
            return HttpPost(GetTiMuParams(jieid), GetTiMuUrl());
        }

        /// <summary>
        /// 保存题目中的图片
        /// </summary>
        /// <param name="str">包含图片的字符串</param>
        /// <returns>图片保存好的字符串</returns>
        public static string imgchuli(string str)
        {
            List<string> imgurl = getImgUrl(str);
            if (imgurl.Count > 0)
            {
                string img = imgurl[0];
                string m_fileName = img.Substring(img.LastIndexOf("/"), img.Length - img.LastIndexOf("/"));
                string m_saveName = "/Areas/cheping/images/upimg/timuimg" + m_fileName;
                string m_savePath = HttpContext.Current.Server.MapPath(m_saveName);
                if (Utils.DownloadPicture(img, m_savePath, 5000))
                    str = str.Replace(img, m_saveName);

                return str;
            }
            else
            {
                return str;
            }
        }


        /// <summary>
        /// 生成获取节参数
        /// </summary>
        /// <param name="jieid">根据章的id获取节的参数</param>
        /// <returns>参数</returns>
        public Dictionary<string, string> GetJieParams(string zhangid)
        {
            Dictionary<string, string> p = new Dictionary<string, string>();
            p.Add("derive", zhangid);
            p.Add("guid", guid);
            p.Add("openid", "");
            p.Add("pwd", pwd);
            p.Add("subjectId", subjectId);
            p.Add("tel", tel);
            return p;
        }

        public List<Jie> GetJies(string zhangid)
        {
            List<Jie> jies = new List<Jie>();
            string str = HttpPost(GetJieParams(zhangid), GetJieUrl());
            jies = JsonConvert.DeserializeObject<List<Jie>>(str);
            return jies;
        }
    }


    /// <summary>
    /// 题目类
    /// </summary>
    public class Question
    {
        public int ErrorCount { get; set; }
        public string VideoName { get; set; }
        public string Collection { get; set; }
        public int QuestionCount { get; set; }
        public double Accuracy { get; set; }
        public string People { get; set; }
        public string Style { get; set; }
        public string EasyExplain { get; set; }
        public string Taxis { get; set; }
        public string Id { get; set; }
        public string Tpi { get; set; }
        public string CollectionAll { get; set; }
        public string Explain { get; set; }
        public string Topic { get; set; }
        public int NotesCount { get; set; }
        public string Title { get; set; }
        public string Answer { get; set; }

        public Question()
        {
            ErrorCount = 0;
            VideoName = "";
            Collection = "";
            QuestionCount = 0;
            Accuracy = 0;
            People = "";
            Style = "";
            EasyExplain = "";
            Taxis = "";
            Id = "";
            Tpi = "";
            CollectionAll = "";
            Explain = "";
            Topic = "";
            NotesCount = 0;
            Title = "";
            Answer = "";
        }


        /// <summary>
        /// 返回改题目的选项
        /// </summary>
        /// <param name="options">爬到的选项s</param>
        /// <returns></returns>
        public List<Options> GetOptions(List<Options> options)
        {
            List<Options> str = new List<Options>();
            foreach (Options item in options)
            {
                if (item.QuestionsId == Id)
                {
                    str.Add(item);
                }
            }
            return str;
        }
    }


    /// <summary>
    /// 选项类
    /// </summary>
    public class Options
    {
        public string Fallibility { get; set; }
        public string QuestionsId { get; set; }
        public string Type { get; set; }
        public string Percentage { get; set; }
        public string IsTrue { get; set; }
        public string Id { get; set; }
        public string Sum { get; set; }
        public string Option { get; set; }
        public Options()
        {
            Fallibility = "";
            QuestionsId = "";
            Type = "";
            Percentage = "";
            IsTrue = "";
            Id = "";
            Sum = "";
            Option = "";
        }

    }


    /// <summary>
    /// 题干类
    /// </summary>
    public class Topic
    {
        public string Answer { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public string Id { get; set; }
        public Topic()
        {
            Answer = "";
            Content = "";
            Title = "";
            Id = "";
        }
    }

    public class Zhang
    {
        public string Id { get; set; }
        public string mingcheng { get; set; }
        public Zhang()
        {
            Id = "";
            mingcheng = "";
        }
    }

    public class Jie
    {
        public string Id { get; set; }
        public string Accomplish { get; set; }
        public string Clildren { get; set; }
        public string Tcount { get; set; }
        public string Derive { get; set; }
        public string Name { get; set; }
        public string Static { get; set; }

        public Jie()
        {
            Id = "";
            Accomplish = "";
            Clildren = "";
            Tcount = "";
            Derive = "";
            Name = "";
            Static = "";
        }

    }
}