using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WeiXinKaifa;

namespace chepingxitong
{
    public enum ShiJuanLeiXing
    {
        作业,
        考试
    }
    public enum ShiJuanZhuangTai
    {
        未发布,
        开始答题,
        继续答题,
        已提交,
        批改中,
        已完成,
        未完成
    }
    public enum KaoShiZhuangTai
    {
        未发布,
        已发布,
        已收卷
    }
    /// <summary>
    /// 题干
    /// </summary>
    public class TiGan
    {
        public string _id { get; set; }
        public string _tigan { get; set; }
        public TiGan()
        {
            _id = "";
            _tigan = "";
        }

        /// <summary>
        /// 根据题干构造一个题干，数据库里有则从数据库里取，无则插入数据库
        /// </summary>
        /// <param name="tigan">题干</param>
        public TiGan(string tigan)
        {
            _tigan = tigan.Trim();
            if (_tigan != "")
            {

                string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
                SqlConnection con = new SqlConnection(mystr);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "select * from tigan where tigan='" + _tigan + "'";
                SqlDataReader mydr = cmd.ExecuteReader();
                if (mydr.Read())
                {
                    _id = mydr["id"].ToString().Trim();
                    mydr.Close();
                }
                else
                {
                    mydr.Close();
                    cmd.CommandText = "insert into tigan(tigan) output inserted.id VALUES (N'" + _tigan + "') ";
                    _id = cmd.ExecuteScalar().ToString();
                }
                con.Close();
                cmd.Dispose();
            }
            else
            {
                _id = "";
            }
        }
        public void genxin()
        {
            if (_id != "")
            {
                string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
                SqlConnection con = new SqlConnection(mystr);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "update tigan set tigan=N'" + Utils.quzhuanyi(_tigan) + "' where id=" + _id;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    con.Close();
                    cmd.Dispose();
                }
                con.Close();
                cmd.Dispose();
            }
        }

        public void shilihua(string id)
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from tigan where id=" + id;
            SqlDataReader mydr = cmd.ExecuteReader();
            if (mydr.Read())
            {
                _tigan = mydr["tigan"].ToString().Trim();
                _id = id;
            }
            mydr.Close();
            con.Close();
            cmd.Dispose();

        }

    }
    /// <summary>
    /// 题目
    /// </summary>
    public class TiMu : IComparable<TiMu>
    {
        public string _id { get; set; }
        public string _leibie { get; set; }
        public string _timu { get; set; }
        public string _tiganid { get; set; }
        public string _jieid { get; set; }
        public int _zuoticishu { get; set; }
        public int _zhengquecishu { get; set; }
        /// <summary>
        /// 1表示需要检查，2表示可显示
        /// </summary>
        public string _zhuangtai { get; set; }
        public string _jiexi { get; set; }
        public string _laiyuan { get; set; }

        public TiMu()
        {
            _id = "";
            _timu = "";
            _jiexi = "";
            _leibie = "";
            _zuoticishu = 0;
            _zhengquecishu = 0;
            _zhuangtai = "";
            _jieid = "";
            _tiganid = "";
            _laiyuan = "";
        }


        /// <summary>
        /// 根据题目id从数据库构造题目
        /// </summary>
        /// <param name="id">题目id</param>
        public TiMu(string id)
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from timu where id=" + id;
            SqlDataReader mydr = cmd.ExecuteReader();
            if (mydr.Read())
            {
                _id = mydr["id"].ToString();
                _leibie = mydr["leibie"].ToString().Trim();
                _timu = mydr["timu"].ToString().Trim();
                _jiexi = Utils.chuli(mydr["jiexi"]);
                _tiganid = Utils.chuli(mydr["tiganid"]);
                _zuoticishu = int.Parse(mydr["zuoticishu"].ToString().Trim());
                _zhengquecishu = int.Parse(mydr["zhengquecishu"].ToString().Trim());
                _zhuangtai = Utils.chuli(mydr["zhuangtai"]);
                _jieid = Utils.chuli(mydr["jieid"]);
                _laiyuan= mydr["laiyuan"].ToString().Trim();
                mydr.Close();
                con.Close();
                cmd.Dispose();

            }
            else
            {
                _id = "";
                _timu = "";
                _jiexi = "";
                _leibie = "";
                _zuoticishu = 0;
                _zhengquecishu = 0;
                _zhuangtai = "";
                _jieid = "";
                _tiganid = "";
                _laiyuan = "";
                mydr.Close();
                con.Close();
                cmd.Dispose();

            }
        }
        public int CompareTo(TiMu obj)
        {
            int result;
            if (this._id == obj._id)
            {
                result = 0;
            }
            else
            {
                if (this._id.CompareTo(obj._id) < 0)
                {
                    result = 1;
                }
                else
                {
                    result = -1;
                }
            }
            return result;
        }



        /// <summary>
        /// 删除  
        /// </summary>
        public Boolean shanchu()
        {
            if (_id != "")
            {
                string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
                SqlConnection con = new SqlConnection(mystr);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "delete from tiku where id=" + _id;
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    con.Close();
                    cmd.Dispose();
                    return false;
                }
                con.Close();
                cmd.Dispose();
                return true;
            }
            else
                return false;
        }



        /// <summary>
        /// 导入数据库
        /// </summary>
        /// <returns></returns>
        public Boolean daoru()
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "INSERT INTO timu(timu,leibie,jiexi,zuoticishu,zhengquecishu,zhuangtai,jieid,laiyuan) output inserted.id values(N'" + Utils.quzhuanyi(_timu) + "','" + _leibie + "',N'" + Utils.quzhuanyi(_jiexi) + "',0,0," + _zhuangtai + "," + _jieid + ",'" + _laiyuan + "')";
            object ii = cmd.ExecuteScalar();
            con.Close();
            cmd.Dispose();
            if (ii != null)
            {
                _id = ii.ToString();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 插入题干
        /// </summary>
        /// <param name="tiganid">题干id</param>
        public void charutigan(string tiganid)
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "update timu set tiganid=" + tiganid + " where id=" + _id;
            cmd.ExecuteNonQuery();
            _tiganid = tiganid;
            con.Close();
            cmd.Dispose();
        }

        /// <summary>
        /// 题目转章节
        /// </summary>
        /// <param name="jieid">转到的节的id</param>
        public void zhuanjie(string jieid)
        {
            if(jieid!=_jieid)
            {
                string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
                SqlConnection con = new SqlConnection(mystr);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();
                Jie yuanjie = new Jie(_jieid);
                Jie xinjie = new Jie(jieid);
                if (_zhuangtai == "2")
                {
                    if (yuanjie._zhangid != xinjie._zhangid)
                    {
                        cmd.CommandText = "update zhang set tiliang2=tiliang2+1 where id=" + xinjie._zhangid;
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "update zhang set tiliang2=tiliang2-1 where id=" + yuanjie._zhangid;
                        cmd.ExecuteNonQuery();
                    }
                    cmd.CommandText = "update jie set tiliang2=tiliang2+1 where id=" + xinjie._id;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "update jie set tiliang2=tiliang2-1 where id=" + yuanjie._id;
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    if (yuanjie._zhangid != xinjie._zhangid)
                    {
                        cmd.CommandText = "update zhang set tiliang1=tiliang1+1 where id=" + xinjie._zhangid;
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "update zhang set tiliang1=tiliang1-1 where id=" + yuanjie._zhangid;
                        cmd.ExecuteNonQuery();
                    }
                    cmd.CommandText = "update jie set tiliang1=tiliang1+1 where id=" + xinjie._id;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "update jie set tiliang1=tiliang1-1 where id=" + yuanjie._id;
                    cmd.ExecuteNonQuery();
                }
                cmd.CommandText = "update timu set jieid=" + xinjie._id + " where id=" + _id;
                cmd.ExecuteNonQuery();
                con.Close();
                cmd.Dispose();
            }
        }

    }

    /// <summary>
    /// 选项
    /// </summary>
    public class XuanXiang
    {
        public string _id { get; set; }
        public string _xuanxiang { get; set; }
        public string _timuid { get; set; }
        public int _xuanzecishu { get; set; }
        public bool _IsTrue { get; set; }

        public XuanXiang()
        {
            _id = "";
            _xuanxiang = "";
            _timuid = "";
            _xuanzecishu = 0;
            _IsTrue = false;
        }


        /// <summary>
        /// 根据id从数据库构造一个选项
        /// </summary>
        /// <param name="id">选项的id</param>
        public XuanXiang(string id)
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from xuanxiang where id=" + id;
            SqlDataReader mydr = cmd.ExecuteReader();
            if (mydr.Read())
            {
                _id = mydr["id"].ToString();
                _xuanxiang = mydr["xuanxiang"].ToString().Trim();
                _timuid = mydr["timuid"].ToString().Trim();
                _xuanzecishu = int.Parse(mydr["xuanzecishu"].ToString().Trim());
                _IsTrue = (bool)mydr["IsTrue"];
                mydr.Close();
                con.Close();
                cmd.Dispose();

            }
            else
            {
                _id = "";
                _xuanxiang = "";
                _timuid = "";
                _xuanzecishu = 0;
                _IsTrue = false;
                mydr.Close();
                con.Close();
                cmd.Dispose();

            }
        }

        /// <summary>
        /// 构造一个选项
        /// </summary>
        /// <param name="xuangxiang">选项</param>
        /// <param name="timuid">题目id</param>
        /// <param name="IsTrue">对还是错</param>
        public XuanXiang(string xuangxiang,string timuid,bool IsTrue)
        {
            _id = "";
            _xuanxiang = xuangxiang;
            _timuid = timuid;
            _xuanzecishu = 0;
            _IsTrue = IsTrue;
        }

        /// <summary>
        /// 将选项导入数据库
        /// </summary>
        public void daorushujuku()
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            int i = 0;
            if (_IsTrue)
                i = 1;
            cmd.CommandText = "INSERT INTO xuanxiang(xuanxiang,timuid,xuanzecishu,IsTrue) output inserted.id values(N'" + Utils.quzhuanyi(_xuanxiang) + "'," + _timuid + ",0," + i + ")";
            object ii = cmd.ExecuteScalar();
            _id = ii.ToString();
            cmd.Dispose();
            con.Close();
        }


    }

    /// <summary>
    /// 单选题
    /// </summary>
    public class DanXuanTi : TiMu
    {
        public List<XuanXiang> _xuanxiangs { get; set; }
        public TiGan _tigan { get; set; }
        public DanXuanTi()
        {
            _tigan = new TiGan();
            _xuanxiangs = new List<XuanXiang>();
        }
        /// <summary>
        /// 根据题目id构造单选题
        /// </summary>
        /// <param name="id">题目的id</param>
        public DanXuanTi(string id) : base(id)
        {
            _xuanxiangs = new List<XuanXiang>();
            _tigan = new TiGan();
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from xuanxiang where timuid=" + _id;
            SqlDataReader mydr = cmd.ExecuteReader();
            while (mydr.Read())
            {
                XuanXiang xx = new XuanXiang(mydr["id"].ToString());
                _xuanxiangs.Add(xx);
            }
            if(_tiganid!="")
            {
                _tigan.shilihua(_tiganid);
            }
            con.Close();
            mydr.Close();
            cmd.Dispose();
        }


        /// <summary>
        /// 构造一个全新单选题并导入数据库
        /// </summary>
        /// <param name="timu">题目</param>
        /// <param name="tiganid">题干id，空字符串表示没有题干</param>
        /// <param name="jieid">所在节的id</param>
        /// <param name="jiexi">解析</param>
        /// <param name="xuangxiangs">题目的选项</param>
        public DanXuanTi(string timu,string tiganid,string jieid,string jiexi,List<XuanXiang> xuangxiangs)
        {
            _id = "";
            _leibie = "单选题";
            _timu = timu;
            _tiganid = tiganid;
            _jieid = jieid;
            _zuoticishu = 0;
            _zhengquecishu = 0;
            _zhuangtai = "2";
            _jiexi = jiexi;
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            if(_tiganid.Trim()!="")
            {
                cmd.CommandText = "INSERT INTO timu(leibie,timu,tiganid,jieid,zuoticishu,zhengquecishu,zhuangtai,jiexi) output inserted.id values('" + _leibie + "',N'" + Utils.quzhuanyi(_timu) + "'," + _tiganid + "," + _jieid + ",0,0,2,N'" + Utils.quzhuanyi(_jiexi) + "')";
            }
            else
            {
                cmd.CommandText = "INSERT INTO timu(leibie,timu,jieid,zuoticishu,zhengquecishu,zhuangtai,jiexi) output inserted.id values('" + _leibie + "',N'" + Utils.quzhuanyi(_timu) + "'," + _jieid + ",0,0,2,N'" + Utils.quzhuanyi(_jiexi) + "')";
            }
            object ii = cmd.ExecuteScalar();
            _id = ii.ToString();
            cmd.Dispose();
            con.Close();
            _xuanxiangs = xuangxiangs;
            foreach(XuanXiang item in xuangxiangs)
            {
                item._timuid = _id;
                item.daorushujuku();
            }
        }

        /// <summary>
        /// 将单选题选项加入考试选项表
        /// </summary>
        /// <param name="kaoshiid">需要加入的考试的id</param>
        public void jinkaoshi(string kaoshiid)
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            foreach(XuanXiang iten in _xuanxiangs)
            {
                cmd.CommandText = "insert into ksxx values(" + kaoshiid + "," + iten._id + ",0)";
                cmd.ExecuteNonQuery();
            }
            con.Close();
            cmd.Dispose();
        }


        
    }

    /// <summary>
    /// 课程
    /// </summary>
    public class KeCheng
    {
        public string _id { get; set; }
        public string _mingcheng { get; set; }
        public int _tiliang { get; set; }
        public string _tupian { get; set; }
        public List<Zhang> _zhangs { get; set; }
        public KeCheng()
        {
            _id = "";
            _mingcheng = "";
            _tiliang = 0;
            _tupian = "";
            _zhangs = new List<Zhang>();
        }


        /// <summary>
        /// 根据课程id从数据库构造课程
        /// </summary>
        /// <param name="id">课程id</param>
        public KeCheng(string id)
        {
            _zhangs = new List<Zhang>();
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from kecheng where id=" + id;
            SqlDataReader mydr = cmd.ExecuteReader();
            if (mydr.Read())
            {
                _id = mydr["id"].ToString();
                _mingcheng = mydr["mingcheng"].ToString().Trim();
                _tupian = mydr["tupian"].ToString().Trim();
                _tiliang = int.Parse(mydr["tiliang"].ToString().Trim());
                mydr.Close();
                con.Close();
                cmd.Dispose();

            }
            else
            {
                _id = "";
                _mingcheng = "";
                _tiliang = 0;
                _tupian = "";
                mydr.Close();
                con.Close();
                cmd.Dispose();

            }
        }

        /// <summary>
        /// 根据课程id实例化课程
        /// </summary>
        /// <param name="id">课程id</param>
        public void shilihua(string id)
        {
            _zhangs = new List<Zhang>();
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from kecheng where id=" + id;
            SqlDataReader mydr = cmd.ExecuteReader();
            if (mydr.Read())
            {
                _id = mydr["id"].ToString();
                _mingcheng = mydr["mingcheng"].ToString().Trim();
                _tupian = mydr["tupian"].ToString().Trim();
                _tiliang = int.Parse(mydr["tiliang"].ToString().Trim());
                mydr.Close();
                cmd.CommandText = "select * from zhang where kechengid=" + _id;
                mydr = cmd.ExecuteReader();
                while (mydr.Read())
                {
                    Zhang z = new Zhang();
                    z.shilihua(mydr["id"].ToString());
                    _zhangs.Add(z);
                }
                mydr.Close();
                con.Close();
                cmd.Dispose();

            }
            else
            {
                _id = "";
                _mingcheng = "";
                _tiliang = 0;
                _tupian = "";
                mydr.Close();
                con.Close();
                cmd.Dispose();

            }
        }


        /// <summary>
        /// 获取该课程下的所有考试
        /// </summary>
        /// <returns>考试列表</returns>
        public List<KaoShi> GetKaoShis()
        {
            List<KaoShi> kaoShis = new List<KaoShi>();
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select id from kaoshi where kechengid='" + _id + "' order by id desc"; 
            cmd.Connection = con;
            con.Open();
            SqlDataReader myDr = cmd.ExecuteReader();
            while (myDr.Read())
            {
                KaoShi kaoshi = new KaoShi(myDr["id"].ToString().Trim());
                kaoShis.Add(kaoshi);

            }
            myDr.Close();
            con.Close();
            cmd.Dispose();
            return kaoShis;
        }
    }

    /// <summary>
    /// 章
    /// </summary>
    public class Zhang
    {
        public string _id { get; set; }
        public string _mingcheng { get; set; }
        public int _tiliang1 { get; set; }
        public int _tiliang2 { get; set; }
        public string _kechengid { get; set; }
        public List<Jie> _jies { get; set; }
        public Zhang()
        {
            _id = "";
            _mingcheng = "";
            _tiliang1 = 0;
            _tiliang2 = 0;
            _kechengid = "";
            _jies = new List<Jie>();
        }


        /// <summary>
        /// 根据章id从数据库构造章
        /// </summary>
        /// <param name="id">章id</param>
        public Zhang(string id)
        {
            _jies = new List<Jie>();
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from zhang where id=" + id;
            SqlDataReader mydr = cmd.ExecuteReader();
            if (mydr.Read())
            {
                _id = mydr["id"].ToString();
                _mingcheng = mydr["mingcheng"].ToString().Trim();
                _kechengid = mydr["kechengid"].ToString().Trim();
                _tiliang1 = int.Parse(mydr["tiliang1"].ToString().Trim());
                _tiliang2 = int.Parse(mydr["tiliang2"].ToString().Trim());
                mydr.Close();
                con.Close();
                cmd.Dispose();

            }
            else
            {
                _id = "";
                _mingcheng = "";
                _tiliang1 = 0;
                _tiliang2 = 0;
                _kechengid = "";
                mydr.Close();
                con.Close();
                cmd.Dispose();

            }
        }

        /// <summary>
        /// 构造一个全新的章
        /// </summary>
        /// <param name="mingcheng">章的名称</param>
        /// <param name="kechengid">章所属课程</param>
        public Zhang(string mingcheng,string kechengid)
        {
            _jies = new List<Jie>();
            _tiliang1 = 0;
            _tiliang2 = 0;
            _mingcheng = mingcheng;
            _kechengid = kechengid;
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "INSERT INTO zhang(mingcheng,tiliang1,tiliang2,kechengid) output inserted.id values('" + _mingcheng + "',0,0," +_kechengid + ")";
            object ii = cmd.ExecuteScalar();
            _id = ii.ToString();
            con.Close();
            cmd.Dispose();
        }

        /// <summary>
        /// 实例化章
        /// </summary>
        /// <param name="id">章id</param>
        public void shilihua(string id)
        {
            _jies = new List<Jie>();
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from zhang where id=" + id;
            SqlDataReader mydr = cmd.ExecuteReader();
            if (mydr.Read())
            {
                _id = mydr["id"].ToString();
                _mingcheng = mydr["mingcheng"].ToString().Trim();
                _kechengid = mydr["kechengid"].ToString().Trim();
                _tiliang1 = int.Parse(mydr["tiliang1"].ToString().Trim());
                _tiliang2 = int.Parse(mydr["tiliang2"].ToString().Trim());
                mydr.Close();
                cmd.CommandText = "select * from jie where zhangid=" + _id;
                mydr = cmd.ExecuteReader();
                while(mydr.Read())
                {
                    _jies.Add(new Jie(mydr["id"].ToString()));
                }
                mydr.Close();
                con.Close();
                cmd.Dispose();

            }
            else
            {
                _id = "";
                _mingcheng = "";
                _tiliang1 = 0;
                _tiliang2 = 0;
                _kechengid = "";
                mydr.Close();
                con.Close();
                cmd.Dispose();

            }
        }


        /// <summary>
        /// 更新题量
        /// </summary>
        public void genxintiliang()
        {
            _tiliang1 = 0;
            _tiliang2 = 0;
            foreach(Jie item in _jies)
            {
                item.genxintiliang();
                _tiliang1 = _tiliang1 + item._tiliang1;
                _tiliang2 = _tiliang2 + item._tiliang2;
            }
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "update zhang set tiliang1=" + _tiliang1 + ",tiliang2=" + _tiliang2 + " where id=" + _id;
            cmd.ExecuteNonQuery();
            con.Close();
            cmd.Dispose();
        }


        /// <summary>
        /// 根据章名，查询数据库，如果有则取出数据。如果无则插入新章
        /// </summary>
        /// <param name="mingcheng">查询的章名</param>
        public void zinenggz(string mingcheng)
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from zhang where mingcheng='" + mingcheng + "'";
            SqlDataReader mydr = cmd.ExecuteReader();
            if(mydr.Read())
            {
                _id = mydr["id"].ToString().Trim();
                _mingcheng = mingcheng;
                _tiliang1 = int.Parse(mydr["tiliang1"].ToString());
                _tiliang2 = int.Parse(mydr["tiliang2"].ToString());
                _kechengid = mydr["kechengid"].ToString().Trim();
                mydr.Close();
            }
            else
            {
                mydr.Close();
                _tiliang1 = 0;
                _tiliang2 = 0;
                _mingcheng = mingcheng;
                _kechengid = "1";
                cmd.CommandText = "INSERT INTO zhang(mingcheng,tiliang1,tiliang2,kechengid) output inserted.id values('" + _mingcheng + "',0,0," + _kechengid + ")";
                object ii = cmd.ExecuteScalar();
                _id = ii.ToString();
            }
            con.Close();
            cmd.Dispose();
        }
    }

    /// <summary>
    /// 节
    /// </summary>
    public class Jie
    {
        public string _id { get; set; }
        public string _mingcheng { get; set; }
        public int _tiliang1 { get; set; }
        public int _tiliang2 { get; set; }
        public string _zhangid { get; set; }
        public Jie()
        {
            _id = "";
            _mingcheng = "";
            _tiliang1 = 0;
            _tiliang2 = 0;
            _zhangid = "";
        }


        /// <summary>
        /// 根据节id从数据库构造节
        /// </summary>
        /// <param name="id">节id</param>
        public Jie(string id)
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from jie where id=" + id;
            SqlDataReader mydr = cmd.ExecuteReader();
            if (mydr.Read())
            {
                _id = mydr["id"].ToString();
                _mingcheng = mydr["mingcheng"].ToString().Trim();
                _zhangid = mydr["zhangid"].ToString().Trim();
                _tiliang1 = int.Parse(mydr["tiliang1"].ToString().Trim());
                _tiliang2 = int.Parse(mydr["tiliang2"].ToString().Trim());
                mydr.Close();
                con.Close();
                cmd.Dispose();

            }
            else
            {
                _id = "";
                _mingcheng = "";
                _tiliang1 = 0;
                _tiliang2 = 0;
                _zhangid = "";
                mydr.Close();
                con.Close();
                cmd.Dispose();

            }
        }

        /// <summary>
        /// 构造一个全新的节
        /// </summary>
        /// <param name="mingcheng">节的名称</param>
        /// <param name="zhangid">节所属章的id</param>
        public Jie(string mingcheng, string zhangid)
        {
            _tiliang1 = 0;
            _tiliang2 = 0;
            _mingcheng = mingcheng;
            _zhangid = zhangid;
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "INSERT INTO jie(mingcheng,tiliang1,tiliang2,zhangid) output inserted.id values('" + _mingcheng + "',0,0," + _zhangid + ")";
            object ii = cmd.ExecuteScalar();
            _id = ii.ToString();
            con.Close();
            cmd.Dispose();
        }

        /// <summary>
        /// 更新题量
        /// </summary>
        public void genxintiliang()
        {
            if(_id!="")
            {
                string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
                SqlConnection con = new SqlConnection(mystr);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "select count(id) from timu where jieid=" + _id + " and zhuangtai=1";
                int tiliang1 = (int)cmd.ExecuteScalar();
                _tiliang1 = tiliang1;
                cmd.CommandText = "select count(id) from timu where jieid=" + _id + " and zhuangtai=2";
                int tiliang2 = (int)cmd.ExecuteScalar();
                _tiliang2 = tiliang2;
                cmd.CommandText = "update jie set tiliang1=" + _tiliang1 + ",tiliang2=" + _tiliang2 + " where id=" + _id;
                cmd.ExecuteNonQuery();
                con.Close();
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 根据章id，和节名称查询数据库，如果有则取出数据。如果无则插入新节
        /// </summary>
        /// <param name="zhangid">章的id</param>
        /// <param name="mingcheng">节的名称</param>
        public void zinenggz(string zhangid,string mingcheng)
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from jie where mingcheng='" + mingcheng + "' and zhangid=" + zhangid;
            SqlDataReader mydr = cmd.ExecuteReader();
            if (mydr.Read())
            {
                _id = mydr["id"].ToString().Trim();
                _mingcheng = mingcheng;
                _tiliang1 = int.Parse(mydr["tiliang1"].ToString());
                _tiliang2 = int.Parse(mydr["tiliang2"].ToString());
                _zhangid = zhangid;
                mydr.Close();
            }
            else
            {
                mydr.Close();
                _tiliang1 = 0;
                _tiliang2 = 0;
                _mingcheng = mingcheng;
                _zhangid = zhangid;
                cmd.CommandText = "INSERT INTO jie(mingcheng,tiliang1,tiliang2,zhangid) output inserted.id values('" + _mingcheng + "',0,0," + _zhangid + ")";
                object ii = cmd.ExecuteScalar();
                _id = ii.ToString();
            }
            con.Close();
            cmd.Dispose();
        }
    }


    /// <summary>
    /// 教师和班级关系的类在教师类中用
    /// </summary>
    public class JSBJ
    {
        public string _banhao { get; set; }
        public string _kechenghao { get; set; }
        public JSBJ()
        {
            _banhao = "";
            _kechenghao = "";
        }
        public JSBJ(string banhao, string kechenghao)
        {
            _banhao = banhao;
            _kechenghao = kechenghao;
        }
    }

    /// <summary>
    /// 教师
    /// </summary>
    public class JiaoShi
    {
        public string _id { get; set; }
        public string _xingming { get; set; }
        public string _username { get; set; }
        public string _shoujihao { get; set; }
        public string _shenfen { get; set; }
        public string _openid { get; set; }
        public string _nicheng { get; set; }
        public string _touxiang { get; set; }
        public int _denglushijian { get; set; }
        public int _genxinshijian { get; set; }
        public List<JSBJ> _jsbjs { get; set; }

        public JiaoShi()
        {
            _id = "";
            _xingming = "";
            _username = "";
            _shoujihao = "";
            _shenfen = "";
            _openid = "";
            _nicheng = "";
            _touxiang = "";
            _jsbjs = new List<JSBJ>();
            _denglushijian = 0;
            _genxinshijian = 0;


        }
        

        /// <summary>
        /// 微信登陆
        /// </summary>
        /// <param name="usein">用户信息</param>
        /// <returns></returns>
        public Boolean login(UserInfo usein)
        {
            if (!string.IsNullOrEmpty(usein.openid))
            {
                string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
                SqlConnection con = new SqlConnection(mystr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from jiaoshi where openid='" + usein.openid + "'";
                cmd.Connection = con;
                con.Open();
                SqlDataReader myDr = cmd.ExecuteReader();

                if (myDr.Read())
                {
                    _id = Utils.chuli(myDr["id"]);
                    _xingming = Utils.chuli(myDr["xingming"]);
                    _username = Utils.chuli(myDr["username"]);
                    _shoujihao = Utils.chuli(myDr["shoujihao"]);
                    _shenfen = Utils.chuli(myDr["shenfen"]);

                    _openid = Utils.chuli(myDr["openid"]);
                    _nicheng = usein.nickname;
                    _touxiang = usein.headimgurl;
                    _denglushijian = Utils.ConvertDateTimeInt(DateTime.Now);
                    int denglushijian;
                    if (Utils.chuli(myDr["denglushijian"]) != "")
                    {
                        denglushijian = int.Parse(Utils.chuli(myDr["denglushijian"]));
                    }
                    if (Utils.chuli(myDr["genxinshijian"]) != "")
                    {
                        _genxinshijian = int.Parse(Utils.chuli(myDr["genxinshijian"]));
                    }

                    myDr.Close();
                    cmd.CommandText = "select * from jsbj where jiaoshiid=" + _id;
                    myDr = cmd.ExecuteReader();
                    while (myDr.Read())
                    {
                        JSBJ jsbj = new JSBJ(myDr["banhao"].ToString().Trim(), myDr["kechenghao"].ToString().Trim());
                        _jsbjs.Add(jsbj);
                    }
                    myDr.Close();
                    if ((_denglushijian - _genxinshijian) >= (86400 * 7) && _touxiang != "")
                    {
                        _genxinshijian = _denglushijian;
                        string m_fileName = _openid + ".jpg";
                        string m_saveName = "/Areas/cheping/images/upimg/jiaoshitouxiang/" + m_fileName;
                        string m_savePath = HttpContext.Current.Server.MapPath(m_saveName);
                        Utils.DeleteFile(m_savePath);
                        Utils.DownloadPicture(usein.headimgurl, m_savePath, -1);
                        cmd.CommandText = "update jiaoshi set denglushijian=" + _denglushijian + ",touxiang='" + m_saveName + "',nicheng='" + _nicheng + "',genxinshijian=" + _genxinshijian + " where openid='" + _openid + "'";
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd.CommandText = "update jiaoshi set denglushijian=" + _denglushijian + " where openid='" + _openid + "'";
                        cmd.ExecuteNonQuery();
                    }

                    con.Close();
                    cmd.Dispose();
                   
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        ///  首次登陆，根据用户名和电话绑定微信
        /// </summary>
        /// <param name="username">学号</param>
        /// <param name="dianhua">电话</param>
        /// <param name="usein">微信信息</param>
        /// <returns></returns>
        public Boolean login(string username, string dianhua, UserInfo usein)
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from jiaoshi where username='" + username + "' and shoujihao='" + dianhua + "'";
            cmd.Connection = con;
            con.Open();
            SqlDataReader myDr = cmd.ExecuteReader();
            if (myDr.Read())
            {
                _id = Utils.chuli(myDr["id"]);
                _xingming = Utils.chuli(myDr["xingming"]);
                _username = Utils.chuli(myDr["username"]);
                _shoujihao = Utils.chuli(myDr["shoujihao"]);
                _shenfen = Utils.chuli(myDr["shenfen"]);
                _openid = usein.openid;
                _nicheng = usein.nickname;
                _touxiang = usein.headimgurl;

                _denglushijian = Utils.ConvertDateTimeInt(DateTime.Now);
                _genxinshijian = _denglushijian;

                myDr.Close();

                cmd.CommandText = "select * from jsbj where jiaoshiid=" + _id;
                myDr = cmd.ExecuteReader();
                while (myDr.Read())
                {
                    JSBJ jsbj = new JSBJ(myDr["banhao"].ToString().Trim(), myDr["kechenghao"].ToString().Trim());
                    _jsbjs.Add(jsbj);
                }
                myDr.Close();
                string m_fileName = _openid + ".jpg";
                string m_saveName = "/Areas/cheping/images/upimg/jiaoshitouxiang/" + m_fileName;
                string m_savePath = HttpContext.Current.Server.MapPath(m_saveName);
                Utils.DeleteFile(m_savePath);
                Utils.DownloadPicture(usein.headimgurl, m_savePath, -1);
                cmd.CommandText = "update jiaoshi set denglushijian=" + _denglushijian + ",touxiang='" + m_saveName + "',nicheng='" + _nicheng + "',genxinshijian=" + _genxinshijian + ",openid='" + _openid + "' where id=" + _id;
                cmd.ExecuteNonQuery();
                con.Close();
                cmd.Dispose();
                
                return true;
            }
            else
            {
                return false;
            }
        }



        public Boolean login(string username, string shoujihao)
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from jiaoshi where username='" + username + "' and shoujihao='" + shoujihao + "'";
            SqlDataReader myDr = cmd.ExecuteReader();
            if (myDr.Read())
            {
                _id = myDr["id"].ToString();
                _xingming = myDr["xingming"].ToString();
                _username = username;
                _shoujihao = shoujihao;
                _shenfen = myDr["shenfen"].ToString();
                _openid = myDr["openid"].ToString();
                _nicheng = myDr["nicheng"].ToString();

                if (myDr["touxiang"] == DBNull.Value)
                {
                    _touxiang = "/img/muorentou.jpg";
                }
                else
                {
                    _touxiang = myDr["touxiang"].ToString();
                }
                if (myDr["denglushijian"] != DBNull.Value)
                {
                    _denglushijian = int.Parse(myDr["denglushijian"].ToString().Trim());
                }
                if (myDr["genxinshijian"] != DBNull.Value)
                {
                    _genxinshijian = int.Parse(myDr["genxinshijian"].ToString().Trim());
                }
                myDr.Close();
                cmd.CommandText = "select * from jsbj where jiaoshiid=" + _id;
                myDr = cmd.ExecuteReader();
                while (myDr.Read())
                {
                    JSBJ jsbj = new JSBJ(myDr["banhao"].ToString().Trim(), myDr["kechenghao"].ToString().Trim());
                    _jsbjs.Add(jsbj);
                }
                myDr.Close();
                cmd.CommandText = "update jiaoshi set denglushijian=" + Utils.ConvertDateTimeInt(DateTime.Now) + " where id=" + _id;
                cmd.ExecuteNonQuery();
                con.Close();
                cmd.Dispose();
                
                return true;
            }
            else
            {
                myDr.Close();
                con.Close();
                cmd.Dispose();
                return false;
            }
        }
        /// <summary>
        /// 获取教师某门课程的班级
        /// </summary>
        /// <param name="kechenghao">教师所上课程的课程号</param>
        /// <returns></returns>
        public List<BanJi> GetBanJis(string kechenghao)
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            List<BanJi> banjis = new List<BanJi>();
            foreach (JSBJ item in _jsbjs)
            {
                if (item._kechenghao == kechenghao)
                {
                    BanJi banji = new BanJi();
                    cmd.CommandText = "select * from banji where banhao='" + item._banhao + "'";
                    SqlDataReader mydr = cmd.ExecuteReader();
                    if (mydr.Read())
                    {
                        banji._banhao = item._banhao;
                        banji._banjiming = mydr["banjiming"].ToString().Trim();
                        banjis.Add(banji);
                    }
                    mydr.Close();

                }
            }

            con.Close();
            cmd.Dispose();
            return banjis;
        }

        /// <summary>
        /// 发布考试
        /// </summary>
        /// <param name="ksid">考试id</param>
        public void fabukaoshi(string ksid,int fabushijian)
        {
            
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "update kaoshi set zhuangtai='" + KaoShiZhuangTai.已发布 + "',fabushijian=" + fabushijian + " where id=" + ksid;
            cmd.ExecuteNonQuery();
            cmd.CommandText = "update shijuan set zhuangtai='" + ShiJuanZhuangTai.开始答题 + "' where kaoshiid=" + ksid;
            cmd.ExecuteNonQuery();
            con.Close();
            cmd.Dispose();

        }


        /// <summary>
        /// 删除考试
        /// </summary>
        /// <param name="ksid">考试id</param>
        public void shanchukaoshi(string ksid)
        {

            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "delete from kaoshi where id=" + ksid;
            cmd.ExecuteNonQuery();
            con.Close();
            cmd.Dispose();

        }

    }


    /// <summary>
    /// 班级
    /// </summary>
    public class BanJi
    {
        public string _banhao { get; set; }
        public string _banjiming { get; set; }
        public string _jiaoxuejihuahao { get; set; }
        public BanJi()
        {
            _banhao = "";
            _banjiming = "";
            _jiaoxuejihuahao = "";
        }
        public BanJi(string banhao, string banjiming, string jiaoxuejihuahao)
        {
            if (banhao.Trim() != "" && banjiming.Trim() != "" && jiaoxuejihuahao.Trim() != "")
            {
                _banhao = banhao.Trim();
                _banjiming = banjiming.Trim();
                _jiaoxuejihuahao = jiaoxuejihuahao.Trim();
            }
        }


        /// <summary>
        /// 通过班号从数据库构造一个班级
        /// </summary>
        /// <param name="banhao">班号</param>
        public  BanJi(string banhao)
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from banji where banhao='" + banhao + "'";
            SqlDataReader mydr = cmd.ExecuteReader();
            if (mydr.Read())
            {
                _banhao = mydr["banhao"].ToString();
                _banjiming = mydr["banjiming"].ToString();
                _jiaoxuejihuahao = mydr["jiaoxuejihuahao"].ToString();
                mydr.Close();
                con.Close();
                cmd.Dispose();
                
            }
            else
            {
                _banhao = "";
                _banjiming = "";
                _jiaoxuejihuahao = "";
                mydr.Close();
                con.Close();
                cmd.Dispose();
               
            }
        }

        /// <summary>
        /// 将班级导入数据库
        /// </summary>
        /// <returns></returns>
        public Boolean daorushujuku()
        {
            if (_banhao != "" && _banjiming != "" && _jiaoxuejihuahao != "")
            {
                string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
                SqlConnection con = new SqlConnection(mystr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO banji VALUES ('" + _banhao + "','" + _banjiming + "','" + _jiaoxuejihuahao + "')";
                cmd.Connection = con;
                con.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    cmd.Dispose();
                    con.Close();
                    return false;
                }
                cmd.Dispose();
                con.Close();
                return true;
            }
            else
                return false;
        }


        /// <summary>
        /// 返回学生s
        /// </summary>
        /// <returns></returns>
        public List<XueSheng> GetXueShengs()
        {
            List<XueSheng> xueshengs = new List<XueSheng>();
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from xuesheng where banhao='" + _banhao + "'";
            SqlDataReader mydr = cmd.ExecuteReader();
            while (mydr.Read())
            {
                XueSheng xs = new XueSheng();
                xs.shilihua(mydr["xuehao"].ToString());
                xueshengs.Add(xs);
            }
            mydr.Close();
            con.Close();
            cmd.Dispose();
            return xueshengs;

        }


        

    }

    /// <summary>
    /// 学生
    /// </summary>
    public class XueSheng
    {
        public string _xuehao { get; set; }
        public string _xingming { get; set; }
        public string _dianhua { get; set; }
        public string _shenfenzhenghao { get; set; }
        public string _banhao { get; set; }
        public string _xueji { get; set; }
        public string _openid { get; set; }
        public string _nicheng { get; set; }
        public string _touxiang { get; set; }
        public string _mingzu { get; set; }
        public string _xingbie { get; set; }
        public int _denglushijian { get; set; }
        public int _genxinshijian { get; set; }
        public string _xinxizhuangtai { get; set; }
        public int _zuotiliang { get; set; }
        public int _zhengqueliang { get; set; }
        public int _weiwanchengkaoshi { get; set; }
        public int _weiwanchengzuoye { get; set; }

        public XueSheng()
        {
            _xuehao = "";
            _xingming = "";
            _dianhua = "";
            _shenfenzhenghao = "";
            _banhao = "";
            _xueji = "";
            _openid = "";
            _nicheng = "";
            _touxiang = "";
            _mingzu = "";
            _xingbie = "";
            _denglushijian = -1;
            _genxinshijian = -1;
            _xinxizhuangtai = "";
            _zuotiliang = 0;
            _zhengqueliang = 0;
            _weiwanchengkaoshi = 0;
            _weiwanchengzuoye = 0;

        }

        public Boolean shilihua(string xuehao)
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from xuesheng where xuehao='" + xuehao + "'";
            SqlDataReader mydr = cmd.ExecuteReader();
            if (mydr.Read())
            {
                _xuehao = Utils.chuli(mydr["xuehao"]);
                _xingming = Utils.chuli(mydr["xingming"]);
                _dianhua = Utils.chuli(mydr["dianhua"]);
                _shenfenzhenghao = Utils.chuli(mydr["shenfenzhenghao"]);
                _banhao = Utils.chuli(mydr["banhao"]);
                _xueji = Utils.chuli(mydr["xueji"]);
                _openid = Utils.chuli(mydr["openid"]);
                _nicheng = Utils.chuli(mydr["nicheng"]);
                if (mydr["touxiang"] == DBNull.Value)
                {
                    _touxiang = "/img/muorentou.jpg";
                }
                else
                {
                    _touxiang = mydr["touxiang"].ToString();
                }
                _mingzu = Utils.chuli(mydr["mingzu"]);
                _xingbie = Utils.chuli(mydr["xingbie"]);
                if (mydr["denglushijian"] != DBNull.Value)
                {
                    _denglushijian = int.Parse(mydr["denglushijian"].ToString().Trim());
                }
                if (mydr["genxinshijian"] != DBNull.Value)
                {
                    _genxinshijian = int.Parse(mydr["genxinshijian"].ToString().Trim());
                }
                _xinxizhuangtai = Utils.chuli(mydr["xuehao"]);
                mydr.Close();

                cmd.CommandText = "select count(*) from tongji where xuehao='" + _xuehao + "'";
                _zuotiliang = (int)cmd.ExecuteScalar();

                cmd.CommandText = "select count(*) from tongji where xuehao='" + _xuehao + "' and zhengquefou=1";
                _zhengqueliang = (int)cmd.ExecuteScalar();

                cmd.CommandText = "select count(*) from shijuan where leixing='" + ShiJuanLeiXing.考试 + "' and xueshengid='" + _xuehao + "' and (zhuangtai='" + ShiJuanZhuangTai.开始答题 + "' or zhuangtai='" + ShiJuanZhuangTai.继续答题 + "')";
                _weiwanchengkaoshi= (int)cmd.ExecuteScalar();

                cmd.CommandText = "select count(*) from shijuan where leixing='" + ShiJuanLeiXing.作业 + "' and xueshengid='" + _xuehao + "' and (zhuangtai='" + ShiJuanZhuangTai.开始答题 + "' or zhuangtai='" + ShiJuanZhuangTai.继续答题 + "')";
                _weiwanchengzuoye = (int)cmd.ExecuteScalar();

                con.Close();
                cmd.Dispose();
                return true;
            }
            else
            {
                mydr.Close();
                con.Close();
                cmd.Dispose();
                return false;
            }
        }


        /// <summary>
        /// 微信登陆
        /// </summary>
        /// <param name="usein">用户信息</param>
        /// <returns></returns>
        public Boolean login(UserInfo usein)
        {
            if (!string.IsNullOrEmpty(usein.openid))
            {
                string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
                SqlConnection con = new SqlConnection(mystr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from xuesheng where openid='" + usein.openid + "'";
                cmd.Connection = con;
                con.Open();
                SqlDataReader myDr = cmd.ExecuteReader();

                if (myDr.Read())
                {
                    _xuehao = Utils.chuli(myDr["xuehao"]);
                    _xingming = Utils.chuli(myDr["xingming"]);
                    _dianhua = Utils.chuli(myDr["dianhua"]);
                    _shenfenzhenghao = Utils.chuli(myDr["shenfenzhenghao"]);
                    _banhao = Utils.chuli(myDr["banhao"]);
                    _xueji = Utils.chuli(myDr["xueji"]);
                    _openid = Utils.chuli(myDr["openid"]);
                    _nicheng = usein.nickname;
                    _touxiang = usein.headimgurl;
                    _mingzu = Utils.chuli(myDr["mingzu"]);


                    if (Utils.chuli(myDr["xingbie"]) == "False")
                    {
                        _xingbie = "女";
                    }
                    else if (Utils.chuli(myDr["xingbie"]) == "True")
                    {
                        _xingbie = "男";
                    }
                    else
                    {
                        _xingbie = "";
                    }

                    _denglushijian = Utils.ConvertDateTimeInt(DateTime.Now);
                    int denglushijian;
                    if (Utils.chuli(myDr["denglushijian"]) != "")
                    {
                        denglushijian = int.Parse(Utils.chuli(myDr["denglushijian"]));
                    }
                    if (Utils.chuli(myDr["genxinshijian"]) != "")
                    {
                        _genxinshijian = int.Parse(Utils.chuli(myDr["genxinshijian"]));
                    }
                    _xinxizhuangtai = Utils.chuli(myDr["xinxizhuangtai"]);
                    myDr.Close();
                    cmd.CommandText = "select count(*) from tongji where xuehao='" + _xuehao + "'";
                    _zuotiliang = (int)cmd.ExecuteScalar();

                    cmd.CommandText = "select count(*) from tongji where xuehao='" + _xuehao + "' and zhengquefou=1";
                    _zhengqueliang = (int)cmd.ExecuteScalar();
                    if ((_denglushijian - _genxinshijian) >= (86400 * 7) && _touxiang != "")
                    {
                        _genxinshijian = _denglushijian;
                        string m_fileName = _openid + ".jpg";
                        string m_saveName = "/Areas/cheping/images/upimg/xueshengtouxiang/" + m_fileName;
                        string m_savePath = HttpContext.Current.Server.MapPath(m_saveName);
                        Utils.DeleteFile(m_savePath);
                        Utils.DownloadPicture(usein.headimgurl, m_savePath, -1);
                        cmd.CommandText = "update xuesheng set denglushijian=" + _denglushijian + ",touxiang='" + m_saveName + "',nicheng='" + _nicheng + "',genxinshijian=" + _genxinshijian + " where openid='" + _openid + "'";
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd.CommandText = "update xuesheng set denglushijian=" + _denglushijian + " where openid='" + _openid + "'";
                        cmd.ExecuteNonQuery();
                    }

                    con.Close();
                    cmd.Dispose();
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }


        /// <summary>
        ///  首次登陆，根据学号和电话绑定微信
        /// </summary>
        /// <param name="xuehao">学号</param>
        /// <param name="dianhua">电话</param>
        /// <param name="usein">微信信息</param>
        /// <returns></returns>
        public Boolean login(string xuehao, string dianhua, UserInfo usein)
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from xuesheng where xuehao='" + xuehao + "' and dianhua='" + dianhua + "'";
            cmd.Connection = con;
            con.Open();
            SqlDataReader myDr = cmd.ExecuteReader();
            if (myDr.Read())
            {
                _xuehao = Utils.chuli(myDr["xuehao"]);
                _xingming = Utils.chuli(myDr["xingming"]);
                _dianhua = Utils.chuli(myDr["dianhua"]);
                _shenfenzhenghao = Utils.chuli(myDr["shenfenzhenghao"]);

                _banhao = Utils.chuli(myDr["banhao"]);
                _xueji = Utils.chuli(myDr["xueji"]);
                _openid = usein.openid;
                _nicheng = usein.nickname;
                _touxiang = usein.headimgurl;
                _mingzu = Utils.chuli(myDr["mingzu"]);
                if (Utils.chuli(myDr["xingbie"]) == "False")
                {
                    _xingbie = "女";
                }
                else if (Utils.chuli(myDr["xingbie"]) == "True")
                {
                    _xingbie = "男";
                }
                else
                {
                    _xingbie = "";
                }

                _denglushijian = Utils.ConvertDateTimeInt(DateTime.Now);
                _genxinshijian = _denglushijian;
                _xinxizhuangtai = Utils.chuli(myDr["xinxizhuangtai"]);
                myDr.Close();
                cmd.CommandText = "select count(*) from tongji where xuehao='" + _xuehao + "'";
                _zuotiliang = (int)cmd.ExecuteScalar();

                cmd.CommandText = "select count(*) from tongji where xuehao='" + _xuehao + "' and zhengquefou=1";
                _zhengqueliang = (int)cmd.ExecuteScalar();
                string m_fileName = _openid + ".jpg";
                string m_saveName = "/Areas/cheping/images/upimg/xueshengtouxiang/" + m_fileName;
                string m_savePath = HttpContext.Current.Server.MapPath(m_saveName);
                Utils.DeleteFile(m_savePath);
                Utils.DownloadPicture(usein.headimgurl, m_savePath, -1);
                cmd.CommandText = "update xuesheng set denglushijian=" + _denglushijian + ",touxiang='" + m_saveName + "',nicheng='" + _nicheng + "',genxinshijian=" + _genxinshijian + ",openid='" + _openid + "' where xuehao='" + _xuehao + "'";
                cmd.ExecuteNonQuery();
                con.Close();
                cmd.Dispose();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获得班级名
        /// </summary>
        /// <returns></returns>
        public string GetBanJiMing()
        {
            if (_banhao != "")
            {
                BanJi banJi = new BanJi(_banhao);
               
                return banJi._banjiming;
            }
            else
                return "";
        }

        /// <summary>
        /// 获取考试的试卷list
        /// </summary>
        /// <returns></returns>
        public List<ShiJuan> GetShiJuans()
        {
            List<ShiJuan> shijuans = new List<ShiJuan>();
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select id from shijuan where xueshengid='" + _xuehao + "' and zhuangtai!='" + ShiJuanZhuangTai.未发布 + "' and leixing='" + ShiJuanLeiXing.考试 + "' order by id desc";
            cmd.Connection = con;
            con.Open();
            SqlDataReader myDr = cmd.ExecuteReader();
            while (myDr.Read())
            {
                ShiJuan shijuan = new ShiJuan(myDr["id"].ToString().Trim());
                shijuans.Add(shijuan);

            }
            myDr.Close();
            con.Close();
            cmd.Dispose();
            return shijuans;
        }


        /// <summary>
        /// 根据试卷id获取试卷
        /// </summary>
        /// <param name="id">试卷的id</param>
        /// <returns></returns>
        public ShiJuan GetShiJuan(string id)
        {
            ShiJuan shijuan = new ShiJuan(id);
            if(shijuan._zhuangtai==ShiJuanZhuangTai.开始答题.ToString())
            {
                shijuan.daojixudati();
                shijuan.getsjtms();
            }
            if(shijuan._zhuangtai==ShiJuanZhuangTai.继续答题.ToString())
            {
                shijuan.getsjtms();
            }
            
            
            return shijuan;
        }

    }

    /// <summary>
    /// 考试类 
    /// </summary>
    public class KaoShi : IComparable<KaoShi>
    {
        public string _id { get; set; }
        public string _mingcheng { get; set; }
        public string _zhuangtai { get; set; }
        public int _fabushijian { get; set; }
        public int _shoujuanshijian { get; set; }
        public int _tijiaoshu { get; set; }
        public int _jigeshu { get; set; }
        public string _jiaoshiid { get; set; }
        public string _kechengid { get; set; }
        public int _shijuanshu { get; set; }
        public int _timushu { get; set; }
        public List<string> _shijuanids { get; set; }
        public List<string> _timuids { get; set; }
        public KaoShi()
        {
            _id = "";
            _mingcheng = "";
            _zhuangtai = "";
            _fabushijian = 0;
            _shoujuanshijian = 0;
            _tijiaoshu = 0;
            _jigeshu = 0;
            _jiaoshiid = "";
            _kechengid = "";
            _shijuanshu = 0;
            _timushu = 0;
            _shijuanids = new List<string>();
            _timuids = new List<string>();
        }


        /// <summary>
        /// 通过考试id从数据库中取考试
        /// </summary>
        /// <param name="id">考试id</param>
        public KaoShi(string id)
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from kaoshi where id=" + id;
            cmd.Connection = con;
            con.Open();
            SqlDataReader myDr = cmd.ExecuteReader();
            if (myDr.Read())
            {
                _id = id;
                _mingcheng = myDr["mingcheng"].ToString().Trim();
                _zhuangtai = myDr["zhuangtai"].ToString().Trim();
                _fabushijian = int.Parse(myDr["fabushijian"].ToString().Trim());
                if (Utils.chuli(myDr["shoujuanshijian"].ToString().Trim()) != "")
                {
                    _shoujuanshijian = int.Parse(Utils.chuli(myDr["shoujuanshijian"].ToString().Trim()));
                }
                if (Utils.chuli(myDr["tijiaoshu"].ToString().Trim()) != "")
                {
                    _tijiaoshu = int.Parse(Utils.chuli(myDr["tijiaoshu"].ToString().Trim()));
                }
                if (Utils.chuli(myDr["jigeshu"].ToString().Trim()) != "")
                {
                    _jigeshu = int.Parse(Utils.chuli(myDr["jigeshu"].ToString().Trim()));
                }
                _jiaoshiid = myDr["jiaoshiid"].ToString().Trim();
                _kechengid = myDr["kechengid"].ToString().Trim();
                _shijuanshu= int.Parse(Utils.chuli(myDr["shijuanshu"].ToString().Trim()));
                _timushu= int.Parse(Utils.chuli(myDr["timushu"].ToString().Trim()));
                _shijuanids = new List<string>();
                _timuids = new List<string>();
            }
            else
            {
                _id = "";
                _mingcheng = "";
                _zhuangtai = "";
                _fabushijian = -1;
                _shoujuanshijian = -1;
                _tijiaoshu = 0;
                _jigeshu = 0;
                _jiaoshiid = "";
                _kechengid = "";
                _shijuanshu = 0;
                _timushu = 0;
                _shijuanids = new List<string>();
                _timuids = new List<string>();
            }
            myDr.Close();
            con.Close();
            cmd.Dispose();
        }
        public int CompareTo(KaoShi obj)
        {


            int result;
            if (this._fabushijian == obj._fabushijian)
            {
                result = 0;
            }
            else
            {
                if (this._fabushijian.CompareTo(obj._fabushijian) < 0)
                {
                    result = 1;
                }
                else
                {
                    result = -1;
                }
            }
            return result;
        }

        
        public KaoShi(string mingcheng, string jiaoshiid, string kechengid,string[] timuids, string[] banhaos)
        {

            _mingcheng = mingcheng;//
            _zhuangtai = KaoShiZhuangTai.未发布.ToString();//
            _fabushijian = 0;//
            _jiaoshiid = jiaoshiid;//
            _kechengid = kechengid;//
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            _shoujuanshijian = 0;//
            _tijiaoshu = 0;//
            _jigeshu = 0;//
            _shijuanids = new List<string>();
            _timuids = new List<string>(timuids);

            _shijuanshu = 0;//
            _timushu = _timuids.Count;//
            cmd.CommandText = "insert into kaoshi(mingcheng,zhuangtai,fabushijian,jiaoshiid,kechengid,shoujuanshijian,tijiaoshu,jigeshu,shijuanshu,timushu) output inserted.id values('" + _mingcheng + "','" + _zhuangtai + "'," + _fabushijian + "," + _jiaoshiid + "," + _kechengid + ",0,0,0,0," + _timushu + ")";
            _id = cmd.ExecuteScalar().ToString();
            foreach (string item in _timuids)
            {
                DanXuanTi dxt = new DanXuanTi(item);
                dxt.jinkaoshi(_id);

            }
            for (int i = 0; i < banhaos.Length; i++)
            {
                BanJi bj = new BanJi(banhaos[i]);
                
                foreach (XueSheng item in bj.GetXueShengs())
                {
                    ShiJuan shijuan = new ShiJuan(_mingcheng, item._xuehao, _id, timuids);
                    _shijuanids.Add(shijuan._id);
                }
            }
            _shijuanshu = _shijuanids.Count;
            cmd.CommandText = "update kaoshi set shijuanshu=" + _shijuanshu + " where id=" + _id;
            cmd.ExecuteNonQuery();
            con.Close();
            cmd.Dispose();

        }

        /// <summary>
        /// 获取试卷ids
        /// </summary>
        public void GetShiJuanIds()
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select id from shijuan where kaoshiid=" + _id;
            cmd.Connection = con;
            con.Open();
            SqlDataReader myDr = cmd.ExecuteReader();
            while (myDr.Read())
            {
                _shijuanids.Add(myDr["id"].ToString().Trim());
            }
            myDr.Close();

            cmd.CommandText = "select timuid from sjtm where shijuanid=" + _shijuanids[0];
            myDr = cmd.ExecuteReader();
            while(myDr.Read())
            {
                _timuids.Add(myDr["timuid"].ToString().Trim());
            }

            myDr.Close();
            con.Close();
            cmd.Dispose();
        }

        

        public void shoujuan()
        {
            _shoujuanshijian = Utils.ConvertDateTimeInt(DateTime.Now);
            List<ShiJuan> shijuans = new List<ShiJuan>();
            if (_id != "")
            {
                _zhuangtai = KaoShiZhuangTai.已收卷.ToString();
                foreach (string item in _shijuanids)
                {
                    ShiJuan sj = new ShiJuan(item);
                    sj.pigai();
                    shijuans.Add(sj);
                    if ((double)sj._zhengqueti/sj._tiliang >= 0.6)
                        _jigeshu++;
                }
                string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
                SqlConnection con = new SqlConnection(mystr);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "update kaoshi set jigeshu=" + _jigeshu + ",zhuangtai='" + KaoShiZhuangTai.已收卷 + "',shoujuanshijian=" + _shoujuanshijian + " where id=" + _id;
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                shijuans.Sort();


                shijuans[0]._mingci = 1;
                cmd.CommandText = "update shijuan set mingci=1 where id=" + shijuans[0]._id;
                cmd.ExecuteNonQuery();
                for (int i = 1; i < shijuans.Count; i++)
                {
                    int n = shijuans[i - 1]._mingci;
                    if (shijuans[i]._zhengqueti == shijuans[i - 1]._zhengqueti)
                    {
                        shijuans[i]._mingci = n;
                    }
                    else
                    {
                        shijuans[i]._mingci = n + 1;
                    }
                    cmd.CommandText = "update shijuan set mingci=" + shijuans[i]._mingci + " where id=" + shijuans[i]._id;
                    cmd.ExecuteNonQuery();

                }
                int maxmingci = shijuans[shijuans.Count - 1]._mingci;
                foreach (ShiJuan item in shijuans)
                {
                    item._paimingzhishu = Math.Round((double)(maxmingci - item._mingci) / (maxmingci - 1), 4);
                    cmd.CommandText = "update shijuan set paimingzhishu=" + item._paimingzhishu + " where id=" + item._id;
                    cmd.ExecuteNonQuery();
                }

                con.Close();
                cmd.Dispose();
            }

        }


        public List<ShiJuan> GetShiJuans()
        {
            List<ShiJuan> shijuans = new List<ShiJuan>();
            if(_shijuanids.Count==0)
            {
                GetShiJuanIds();
            }
            foreach(string shijuanid in _shijuanids)
            {
                ShiJuan sj = new ShiJuan(shijuanid);
                shijuans.Add(sj);
            }
            return shijuans;
        }


    }

    public class ShiJuan : IComparable<ShiJuan>
    {
        public string _id { get; set; }
        public string _mingcheng { get; set; }
        public string _zhuangtai { get; set; }
        public int _tijiaoshijian { get; set; }
        public string _xueshengid { get; set; }
        public string _kaoshiid { get; set; }
        public int _mingci { get; set; }
        public double _paimingzhishu { get; set; }
        public int _tiliang { get; set; }
        public int _yizuoti { get; set; }
        public int _zhengqueti { get; set; }
        public string _leixing { get; set; }
        public List<SJTM> _timus { get; set; }
        public int _fabushijian { get; set; }
        public int CompareTo(ShiJuan obj)
        {


            int result;
            if (this._zhengqueti == obj._zhengqueti)
            {
                result = 0;
            }
            else
            {
                if (this._zhengqueti.CompareTo(obj._zhengqueti) < 0)
                {
                    result = 1;
                }
                else
                {
                    result = -1;
                }
            }
            return result;

        }

        
        /// <summary>
        /// 试卷
        /// </summary>
        public ShiJuan()
        {
            _id = "";
            _mingcheng = "";
            _zhuangtai = "";
            _tijiaoshijian =0;
            
            _xueshengid = "";
            _kaoshiid = "";
            
            _mingci = -1;
            _paimingzhishu = -1;
            _tiliang = 0;
            _yizuoti = 0;
            _zhengqueti = 0;
            _leixing = "";
            _timus = new List<SJTM>();
            _fabushijian = 0;

        }
        

        /// <summary>
        /// 根据考试生成一个试卷
        /// </summary>
        /// <param name="mingcheng">试卷的名称</param>
        /// <param name="xueshengid">学生的id</param>
        /// <param name="kaoshiid">考试的id</param>
        /// <param name="timuids">题目的ids</param>
        public ShiJuan(string mingcheng,string xueshengid, string kaoshiid, string[] timuids)
        {
            _mingcheng = mingcheng;
            _zhuangtai = ShiJuanZhuangTai.未发布.ToString();
            _tijiaoshijian = -1;
            _xueshengid = xueshengid;
            _kaoshiid = kaoshiid;
            _tiliang = timuids.Length;
            _yizuoti = 0;
            _zhengqueti = 0;
            _leixing = ShiJuanLeiXing.考试.ToString();
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO shijuan(mingcheng,zhuangtai,tijiaoshijian,xueshengid,kaoshiid,tiliang,zhengqueti,leixing) output inserted.id VALUES ('" + _mingcheng + "','" + _zhuangtai + "'," + _tijiaoshijian + ",'" + _xueshengid + "'," + _kaoshiid + "," + _tiliang + ",0,'" + _leixing + "')";
            cmd.Connection = con;
            con.Open();
            _id = cmd.ExecuteScalar().ToString();
            
            
            for(int i=0;i<timuids.Length;i++)
            {
                cmd.CommandText = "insert into sjtm(shijuanid,timuid) values(" + _id + "," + timuids[i] + ")";
                cmd.ExecuteNonQuery();
            }
            cmd.Dispose();
            con.Close();
        }


        /// <summary>
        /// 通过试卷号从数据库构造一个试卷
        /// </summary>
        /// <param name="shijuanhao">试卷号</param>
        public ShiJuan(string shijuanhao)
        {
            _timus = new List<SJTM>();
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from shijuan where id=" + shijuanhao;
            cmd.Connection = con;
            con.Open();
            SqlDataReader myDr = cmd.ExecuteReader();
            if (myDr.Read())
            {
                _id = shijuanhao;
                _mingcheng= myDr["mingcheng"].ToString().Trim();
                _zhuangtai = myDr["zhuangtai"].ToString().Trim();
                _tijiaoshijian = int.Parse(Utils.chuli(myDr["tijiaoshijian"]));
                _xueshengid = Utils.chuli(myDr["xueshengid"]);
                _kaoshiid = Utils.chuli(myDr["kaoshiid"]);
                
                if (Utils.chuli(myDr["mingci"]) != "")
                {
                    _mingci = int.Parse(Utils.chuli(myDr["mingci"]));
                }
                if (Utils.chuli(myDr["paimingzhishu"]) != "")
                {
                    _paimingzhishu = double.Parse(Utils.chuli(myDr["paimingzhishu"]));
                }
                _tiliang = int.Parse(Utils.chuli(myDr["tiliang"]));
                
                _zhengqueti = int.Parse(Utils.chuli(myDr["zhengqueti"]));
                _leixing= myDr["leixing"].ToString().Trim();

                KaoShi ks = new KaoShi(_kaoshiid);
                _fabushijian = ks._fabushijian;
                myDr.Close();

                cmd.CommandText = "select count(*) from sjtm where shijuanid=" + _id + " and xuanxiangid is not null";
                _yizuoti = (int)cmd.ExecuteScalar();
                con.Close();
                cmd.Dispose();
              
            }
            else
            {
                _id = "";
                _mingcheng = "";
                _zhuangtai = "";
                _tijiaoshijian = 0;

                _xueshengid = "";
                _kaoshiid = "";

                _mingci = -1;
                _paimingzhishu = -1;
                _tiliang = 0;
                _yizuoti = 0;
                _zhengqueti = 0;
                _leixing = "";
                _fabushijian = 0;
                myDr.Close();
                con.Close();
                cmd.Dispose();
               
            }
        }

        /// <summary>
        /// 给该试卷的试卷题目赋值
        /// </summary>
        public void getsjtms()
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from sjtm where shijuanid=" + _id;
            cmd.Connection = con;
            con.Open();
            SqlDataReader myDr = cmd.ExecuteReader();
            while (myDr.Read())
            {
                SJTM sjtm = new SJTM(_id, myDr["timuid"].ToString().Trim());
                _timus.Add(sjtm);
            }
        }


        /// <summary>
        /// 返回学生
        /// </summary>
        /// <returns></returns>
        public XueSheng GetXueSheng()
        {
            XueSheng xs = new XueSheng();
            xs.shilihua(_xueshengid);
            return xs;
        }


        /// <summary>
        /// 试卷状态改为继续答题
        /// </summary>
        public void daojixudati()
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "update shijuan set zhuangtai='" + ShiJuanZhuangTai.继续答题 + "' where id=" + _id;
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            cmd.Dispose();
        }

        /// <summary>
        /// 试卷批改
        /// </summary>
        public void pigai()
        {
            if (_zhuangtai != "已完成" && _zhuangtai != "未完成"&&_zhuangtai!="未发布")
            {
                int dangqianshijian = Utils.ConvertDateTimeInt(DateTime.Now);
                string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
                SqlConnection con = new SqlConnection(mystr);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();
                if (_zhuangtai == "继续答题" || _zhuangtai == "已提交")
                {
                    if(((double)_yizuoti/_tiliang)>=0.6)
                    {
                        _zhuangtai = ShiJuanZhuangTai.已完成.ToString();
                    }
                    else
                    {
                        _zhuangtai = ShiJuanZhuangTai.未完成.ToString();
                    }
                    if (_timus.Count == 0)
                        getsjtms();
                    foreach(SJTM item in _timus)
                    {
                        if (item._xuanxiangid != "")
                        {
                            cmd.CommandText = "update xuanxiang set xuanzecishu=xuanzecishu+1 where id=" + item._xuanxiangid;
                            cmd.ExecuteNonQuery();//选项的选择次数加1
                            if (item._xuanxiang._IsTrue)
                            {
                                _zhengqueti++;
                                cmd.CommandText = "if not exists(select * from tongji where xuehao = '" + _xueshengid + "' and timuid=" + item._timuid + ") INSERT INTO tongji VALUES('" + _xueshengid + "'," + item._timuid + ",1," + dangqianshijian + ")";
                                cmd.ExecuteNonQuery();//进统计

                                cmd.CommandText = "update timu set zuoticishu=zuoticishu+1,zhengquecishu=zhengquecishu+1 where id=" + item._timuid;
                                cmd.ExecuteNonQuery();//进题目统计
                            }
                            else
                            {
                                cmd.CommandText = "if not exists(select * from tongji where xuehao = '" + _xueshengid + "' and timuid=" + item._timuid + ") INSERT INTO tongji VALUES('" + _xueshengid + "'," + item._timuid + ",0," + dangqianshijian + ")";
                                cmd.ExecuteNonQuery();//进统计

                                cmd.CommandText = "update timu set zuoticishu=zuoticishu+1 where id=" + item._timuid;
                                cmd.ExecuteNonQuery();//进题目统计
                            }

                            if(_kaoshiid!="")//进考试选项
                            {
                                cmd.CommandText = "update ksxx set xuanzecishu=xuanzecishu+1 where kaoshiid=" + _kaoshiid + " and xuanxiangid=" + item._xuanxiangid;
                                cmd.ExecuteNonQuery();//选项的选择次数加1
                            }
                        }
                    }
                }
                else if (_zhuangtai == "开始答题")
                {
                    _zhuangtai = "未完成";
                }
                cmd.CommandText = "update shijuan set zhuangtai='" + _zhuangtai + "',zhengqueti=" + _zhengqueti + " where id=" + _id;
                cmd.ExecuteNonQuery();//试卷状态更改


                con.Close();
                cmd.Dispose();

            }
        }
    }

    /// <summary>
    /// 试卷题目
    /// </summary>
    public class SJTM
    {
        
        public string _timuid { get; set; }
        public string _xuanxiangid { get; set; }
        public XuanXiang _xuanxiang { get; set; }
        public DanXuanTi _danxuanti { get; set; }
        public SJTM()
        {
            
            _timuid = "";
            _xuanxiangid = "";
            _xuanxiang = new XuanXiang();
            _danxuanti = new DanXuanTi();
        }


        /// <summary>
        /// 从数据库构造sjtm
        /// </summary>
        /// <param name="shijuanid"></param>
        /// <param name="timuid"></param>
        public SJTM(string shijuanid,string timuid)
        {
            
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from sjtm where shijuanid=" + shijuanid + " and timuid=" + timuid;
            cmd.Connection = con;
            con.Open();
            SqlDataReader myDr = cmd.ExecuteReader();
            if (myDr.Read())
            {
                _timuid = timuid;
                _xuanxiangid = Utils.chuli(myDr["xuanxiangid"]);
                if(_xuanxiangid!="")
                {
                    _xuanxiang = new XuanXiang(_xuanxiangid);

                }
                else
                {
                    _xuanxiang = new XuanXiang();
                }
                _danxuanti = new DanXuanTi(_timuid);
            }
            else
            {
                _timuid = "";
                _xuanxiangid = "";
                _xuanxiang = new XuanXiang();
                _danxuanti = new DanXuanTi();
            }
            myDr.Close();
            con.Close();
            cmd.Dispose();

        }
    }

    /// <summary>
    /// 统计考试的情况
    /// </summary>
    public class KaoShiTiMuTongJi
    {
        public DanXuanTi _timu { get; set; }
        public int _zuoticishu { get; set; }
        public int _zhengquecishu { get; set; }
        public List<KSXX> _ksxxs { get; set; }
        public KaoShiTiMuTongJi()
        {
            _timu = new DanXuanTi();
            _zuoticishu = 0;
            _zhengquecishu = 0;
            _ksxxs = new List<KSXX>();
        }
        public KaoShiTiMuTongJi(string kaoshiid, string timuid)
        {
            _timu = new DanXuanTi(timuid);
            _ksxxs = new List<KSXX>();
            foreach (XuanXiang item in _timu._xuanxiangs)
            {
                KSXX ksxx = new KSXX(kaoshiid, item._id);
                _zuoticishu = _zuoticishu + ksxx._xuanzecishu;
                if(item._IsTrue)
                {
                    _zhengquecishu = ksxx._xuanzecishu;
                }
                _ksxxs.Add(ksxx);
            }
        }
    }

    public class KSXX
    {
        public string _kaoshiid { get; set; }
        public string _xuanxiangid { get; set; }
        public int _xuanzecishu { get; set; }
        public KSXX()
        {
            _kaoshiid = "";
            _xuanxiangid = "";
            _xuanzecishu = -1;
        }
        public KSXX(string kaoshiid,string xuanxiangid)
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString4"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "select * from ksxx where kaoshiid=" + kaoshiid + " and xuanxiangid=" + xuanxiangid;
            cmd.Connection = con;
            con.Open();
            SqlDataReader myDr = cmd.ExecuteReader();

            if (myDr.Read())
            {
                _kaoshiid = kaoshiid;
                _xuanxiangid = xuanxiangid;
                _xuanzecishu = int.Parse(myDr["xuanzecishu"].ToString());
            }
            else
            {
                _kaoshiid = "";
                _xuanxiangid = "";
                _xuanzecishu = -1;
            }
            myDr.Close();
            cmd.Dispose();
            con.Close();
        }
    }
}