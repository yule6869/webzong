using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WeiXinKaifa;

namespace sqjymodel
{
    public class XinWen : IComparable<XinWen>
    {
        public string _id { get; set; }
        public string _biaoti { get; set; }
        public int _shijian { get; set; }
        public string _shijian2 { get; set; }
        public string _zuozhe { get; set; }
        public int _liulanliang { get; set; }
        public string _leirong { get; set; }
        public string _leixing { get; set; }
        public string _jianjie { get; set; }
        public string _fenmiantupian { get; set; }
        public string _shifouxianshi { get; set; }
        public int CompareTo(XinWen obj)
        {


            int result;
            if (this._shijian == obj._shijian)
            {
                result = 0;
            }
            else
            {
                if (this._shijian.CompareTo(obj._shijian) < 0)
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

        public XinWen()
        {
            _id = "";
            _biaoti = "";
            _shijian = 0;
            _shijian2 = "";
            _zuozhe = "";
            _liulanliang = 0;
            _leirong = "";
            _leixing = "";
            _jianjie = "";
            _fenmiantupian = "";
            _shifouxianshi = "";
        }

        public void shilihua(string id)
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString2"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from xinwen where id=" + id;
            SqlDataReader mydr = cmd.ExecuteReader();
            if (mydr.Read())
            {
                _id = id;
                _biaoti = Utils.chuli(mydr["biaoti"]);
                _shijian = int.Parse(Utils.chuli(mydr["shijian"]));
                _shijian2 = Utils.chuli(mydr["shijian2"]);
                _zuozhe = Utils.chuli(mydr["zuozhe"]);
                _liulanliang = int.Parse(Utils.chuli(mydr["liulanliang"]));
                _leirong = Utils.chuli(mydr["leirong"]);
                _leixing = Utils.chuli(mydr["leixing"]);
                _jianjie = Utils.chuli(mydr["jianjie"]);
                _fenmiantupian = Utils.chuli(mydr["fenmiantupian"]);
                _shifouxianshi = Utils.chuli(mydr["shifouxianshi"]);
                con.Close();
                cmd.Dispose();

            }
            else
            {
                mydr.Close();
                con.Close();
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 浏览次数加一次
        /// </summary>
        public void jiayi()
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString2"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "update xinwen set liulanliang=liulanliang+1 where id=" + _id;
            cmd.ExecuteNonQuery();
            con.Close();
            cmd.Dispose();
        }
    }

    public class KeCheng
    {
        public string _id { get; set; }
        public string _kechengming { get; set; }
        public string _kechengleixing { get; set; }
        public string _kechengjianjie { get; set; }
        public string _zhujiangjiaoshi { get; set; }
        public string _kechengtupian { get; set; }
        public string _zhidingtupian { get; set; }
        public string _shangchuanshijian { get; set; }
        public int _liulanliang { get; set; }
        public string _shifouxianshi { get; set; }
        public string _zhiding { get; set; }

        public List<ShiPing> _shipings { get; set; }

        public KeCheng()
        {
            _id = "";
            _kechengming = "";
            _kechengleixing = "";
            _kechengjianjie = "";
            _zhujiangjiaoshi = "";
            _kechengtupian = "";
            _zhidingtupian = "";
            _shangchuanshijian = "";
            _liulanliang = 0;
            _shifouxianshi = "";
            _zhiding = "";
            _shipings = new List<ShiPing>();
        }

        public void shilihua(string id)
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString2"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from kecheng where id=" + id;
            SqlDataReader mydr = cmd.ExecuteReader();
            if (mydr.Read())
            {
                _id = id;
                _kechengming = Utils.chuli(mydr["kechengming"]);
                _kechengleixing = Utils.chuli(mydr["kechengleixing"]);
                _kechengjianjie = Utils.chuli(mydr["kechengjianjie"]);
                _kechengtupian = Utils.chuli(mydr["kechengtupian"]);
                _zhidingtupian = Utils.chuli(mydr["zhidingtupian"]);
                _shangchuanshijian = Utils.chuli(mydr["shangchuanshijian"]);
                _liulanliang = int.Parse(Utils.chuli(mydr["liulanliang"]));
                _zhiding = Utils.chuli(mydr["zhiding"]);
                _shifouxianshi = Utils.chuli(mydr["shifouxianshi"]);
                _shipings = Utils.SqlstrToModels<ShiPing>("SELECT shiping.* FROM shiping INNER JOIN kcsp ON shiping.id = kcsp.shipingid INNER JOIN kecheng ON kcsp.kechengid = kecheng.id WHERE kecheng.id = " + _id + " ORDER BY kcsp.xuhao", mystr);
                mydr.Close();
                con.Close();
                cmd.Dispose();

            }
            else
            {
                mydr.Close();
                con.Close();
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 浏览次数加一次
        /// </summary>
        public void jiayi()
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString2"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "update kecheng set liulanliang=liulanliang+1 where id=" + _id;
            cmd.ExecuteNonQuery();
            con.Close();
            cmd.Dispose();
        }
    }

    public class ShiPing
    {
        public string _id { get; set; }
        public string _shipingming { get; set; }
        public string _shipingtu { get; set; }
        public string _shipingdizhi { get; set; }

        public ShiPing()
        {
            _id = "";
            _shipingming = "";
            _shipingdizhi = "";
            _shipingtu = "";
        }

        public void shilihua(string id)
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString2"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from shiping where id=" + id;
            SqlDataReader mydr = cmd.ExecuteReader();
            if (mydr.Read())
            {
                _id = id;
                _shipingming = Utils.chuli(mydr["shipingming"]);
                _shipingdizhi = Utils.chuli(mydr["shipingdizhi"]);
                _shipingtu = Utils.chuli(mydr["shipingtu"]);
                
                con.Close();
                cmd.Dispose();

            }
            else
            {
                mydr.Close();
                con.Close();
                cmd.Dispose();
            }
        }
    }

    public class Admin
    {
        public string _yonghuming { get; set; }
        public string _mima { get; set; }

        public Admin()
        {
            _yonghuming = "";
            _mima = "";
        }

        public Boolean Login(string yonghuming,string mima)
        {
            string mystr = ConfigurationManager.AppSettings["ConnectionString2"].ToString();
            SqlConnection con = new SqlConnection(mystr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from admin where yonghuming='" + yonghuming + "' and mima='" + mima + "'";
            SqlDataReader mydr = cmd.ExecuteReader();
            if (mydr.Read())
            {
                _yonghuming = yonghuming;
                _mima = mima;
                mydr.Close();
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
    }
}