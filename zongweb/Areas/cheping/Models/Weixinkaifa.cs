using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OfficeOpenXml;
using System.Web.Security;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;

/// <summary>
/// Weixinkaifa 的摘要说明
/// </summary>
/// 
namespace WeiXinKaifa
{
    public class Validator
    {
        #region 匹配方法
        /// <summary>  
        /// 验证字符串是否匹配正则表达式描述的规则  
        /// </summary>  
        /// <param name="inputStr">待验证的字符串</param>  
        /// <param name="patternStr">正则表达式字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsMatch(string inputStr, string patternStr)
        {
            return IsMatch(inputStr, patternStr, false, false);
        }

        /// <summary>  
        /// 验证字符串是否匹配正则表达式描述的规则  
        /// </summary>  
        /// <param name="inputStr">待验证的字符串</param>  
        /// <param name="patternStr">正则表达式字符串</param>  
        /// <param name="ifIgnoreCase">匹配时是否不区分大小写</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsMatch(string inputStr, string patternStr, bool ifIgnoreCase)
        {
            return IsMatch(inputStr, patternStr, ifIgnoreCase, false);
        }

        /// <summary>  
        /// 验证字符串是否匹配正则表达式描述的规则  
        /// </summary>  
        /// <param name="inputStr">待验证的字符串</param>  
        /// <param name="patternStr">正则表达式字符串</param>  
        /// <param name="ifValidateWhiteSpace">是否验证空白字符串</param>  
        /// <returns>是否匹配</returns>  
        //public static bool IsMatch(string inputStr, string patternStr, bool ifValidateWhiteSpace)
        //{
        //    return IsMatch(inputStr, patternStr, false, ifValidateWhiteSpace);
        //}

        /// <summary>  
        /// 验证字符串是否匹配正则表达式描述的规则  
        /// </summary>  
        /// <param name="inputStr">待验证的字符串</param>  
        /// <param name="patternStr">正则表达式字符串</param>  
        /// <param name="ifIgnoreCase">匹配时是否不区分大小写</param>  
        /// <param name="ifValidateWhiteSpace">是否验证空白字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsMatch(string inputStr, string patternStr, bool ifIgnoreCase, bool ifValidateWhiteSpace)
        {
            if (!ifValidateWhiteSpace && string.IsNullOrWhiteSpace(inputStr))//.NET 4.0 新增IsNullOrWhiteSpace 方法，便于对用户做处理
                return false;//如果不要求验证空白字符串而此时传入的待验证字符串为空白字符串，则不匹配  
            Regex regex = null;
            if (ifIgnoreCase)
                regex = new Regex(patternStr, RegexOptions.IgnoreCase);//指定不区分大小写的匹配  
            else
                regex = new Regex(patternStr);
            return regex.IsMatch(inputStr);
        }
        #endregion

        #region 验证方法
        /// <summary>  
        /// 验证数字(double类型)  
        /// [可以包含负号和小数点]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsNumber(string input)
        {
            //string pattern = @"^-?\d+$|^(-?\d+)(\.\d+)?$";  
            //return IsMatch(input, pattern);  
            double d = 0;
            if (double.TryParse(input, out d))
                return true;
            else
                return false;
        }

        /// <summary>  
        /// 验证整数  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsInteger(string input)
        {
            //string pattern = @"^-?\d+$";  
            //return IsMatch(input, pattern);  
            int i = 0;
            if (int.TryParse(input, out i))
                return true;
            else
                return false;
        }

        /// <summary>  
        /// 验证非负整数  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsIntegerNotNagtive(string input)
        {
            //string pattern = @"^\d+$";  
            //return IsMatch(input, pattern);  
            int i = -1;
            if (int.TryParse(input, out i) && i >= 0)
                return true;
            else
                return false;
        }

        /// <summary>  
        /// 验证正整数  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsIntegerPositive(string input)
        {
            //string pattern = @"^[0-9]*[1-9][0-9]*$";  
            //return IsMatch(input, pattern);  
            int i = 0;
            if (int.TryParse(input, out i) && i >= 1)
                return true;
            else
                return false;
        }

        /// <summary>  
        /// 验证小数  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsDecimal(string input)
        {
            string pattern = @"^([-+]?[1-9]\d*\.\d+|-?0\.\d*[1-9]\d*)$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证只包含英文字母  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsEnglishCharacter(string input)
        {
            string pattern = @"^[A-Za-z]+$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证只包含数字和英文字母  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsIntegerAndEnglishCharacter(string input)
        {
            string pattern = @"^[0-9A-Za-z]+$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证只包含汉字  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsChineseCharacter(string input)
        {
            string pattern = @"^[\u4e00-\u9fa5]+$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证数字长度范围（数字前端的0计长度）  
        /// [若要验证固定长度，可传入相同的两个长度数值]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <param name="lengthBegin">长度范围起始值（含）</param>  
        /// <param name="lengthEnd">长度范围结束值（含）</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsIntegerLength(string input, int lengthBegin, int lengthEnd)
        {
            //string pattern = @"^\d{" + lengthBegin + "," + lengthEnd + "}$";  
            //return IsMatch(input, pattern);  
            if (input.Length >= lengthBegin && input.Length <= lengthEnd)
            {
                int i;
                if (int.TryParse(input, out i))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>  
        /// 验证字符串包含内容  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <param name="withEnglishCharacter">是否包含英文字母</param>  
        /// <param name="withNumber">是否包含数字</param>  
        /// <param name="withChineseCharacter">是否包含汉字</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsStringInclude(string input, bool withEnglishCharacter, bool withNumber, bool withChineseCharacter)
        {
            if (!withEnglishCharacter && !withNumber && !withChineseCharacter)
                return false;//如果英文字母、数字和汉字都没有，则返回false  
            StringBuilder patternString = new StringBuilder();
            patternString.Append("^[");
            if (withEnglishCharacter)
                patternString.Append("a-zA-Z");
            if (withNumber)
                patternString.Append("0-9");
            if (withChineseCharacter)
                patternString.Append(@"\u4E00-\u9FA5");
            patternString.Append("]+$");
            return IsMatch(input, patternString.ToString());
        }

        /// <summary>  
        /// 验证字符串长度范围  
        /// [若要验证固定长度，可传入相同的两个长度数值]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <param name="lengthBegin">长度范围起始值（含）</param>  
        /// <param name="lengthEnd">长度范围结束值（含）</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsStringLength(string input, int lengthBegin, int lengthEnd)
        {
            //string pattern = @"^.{" + lengthBegin + "," + lengthEnd + "}$";  
            //return IsMatch(input, pattern);  
            if (input.Length >= lengthBegin && input.Length <= lengthEnd)
                return true;
            else
                return false;
        }

        /// <summary>  
        /// 验证字符串长度范围（字符串内只包含数字和/或英文字母）  
        /// [若要验证固定长度，可传入相同的两个长度数值]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <param name="lengthBegin">长度范围起始值（含）</param>  
        /// <param name="lengthEnd">长度范围结束值（含）</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsStringLengthOnlyNumberAndEnglishCharacter(string input, int lengthBegin, int lengthEnd)
        {
            string pattern = @"^[0-9a-zA-z]{" + lengthBegin + "," + lengthEnd + "}$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证字符串长度范围（字符串内只包含英文字母）  
        /// [若要验证固定长度，可传入相同的两个长度数值]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <param name="lengthBegin">长度范围起始值（含）</param>  
        /// <param name="lengthEnd">长度范围结束值（含）</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsStringLengthOnlyEnglishCharacter(string input, int lengthBegin, int lengthEnd)
        {
            string pattern = @"^[a-zA-z]{" + lengthBegin + "," + lengthEnd + "}$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证字符串长度范围  
        /// [若要验证固定长度，可传入相同的两个长度数值]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <param name="withEnglishCharacter">是否包含英文字母</param>  
        /// <param name="withNumber">是否包含数字</param>  
        /// <param name="withChineseCharacter">是否包含汉字</param>  
        /// <param name="lengthBegin">长度范围起始值（含）</param>  
        /// <param name="lengthEnd">长度范围结束值（含）</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsStringLengthByInclude(string input, bool withEnglishCharacter, bool withNumber, bool withChineseCharacter, int lengthBegin, int lengthEnd)
        {
            if (!withEnglishCharacter && !withNumber && !withChineseCharacter)
                return false;//如果英文字母、数字和汉字都没有，则返回false  
            StringBuilder patternString = new StringBuilder();
            patternString.Append("^[");
            if (withEnglishCharacter)
                patternString.Append("a-zA-Z");
            if (withNumber)
                patternString.Append("0-9");
            if (withChineseCharacter)
                patternString.Append(@"\u4E00-\u9FA5");
            patternString.Append("]{" + lengthBegin + "," + lengthEnd + "}$");
            return IsMatch(input, patternString.ToString());
        }

        /// <summary>  
        /// 验证字符串字节数长度范围  
        /// [若要验证固定长度，可传入相同的两个长度数值；每个汉字为两个字节长度]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <param name="lengthBegin">长度范围起始值（含）</param>  
        /// <param name="lengthEnd">长度范围结束值（含）</param>  
        /// <returns></returns>  
        public static bool IsStringByteLength(string input, int lengthBegin, int lengthEnd)
        {
            //int byteLength = Regex.Replace(input, @"[^\x00-\xff]", "ok").Length;  
            //if (byteLength >= lengthBegin && byteLength <= lengthEnd)  
            //{  
            //    return true;  
            //}  
            //return false;  
            int byteLength = Encoding.Default.GetByteCount(input);
            if (byteLength >= lengthBegin && byteLength <= lengthEnd)
                return true;
            else
                return false;
        }

        /// <summary>  
        /// 验证日期  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsDateTime(string input)
        {
            DateTime dt;
            if (DateTime.TryParse(input, out dt))
                return true;
            else
                return false;
        }

        /// <summary>  
        /// 验证固定电话号码  
        /// [3位或4位区号；区号可以用小括号括起来；区号可以省略；区号与本地号间可以用减号或空格隔开；可以有3位数的分机号，分机号前要加减号]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsTelePhoneNumber(string input)
        {
            string pattern = @"^(((0\d2|0\d{2})[- ]?)?\d{8}|((0\d3|0\d{3})[- ]?)?\d{7})(-\d{3})?$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证手机号码  
        /// [可匹配"(+86)013325656352"，括号可以省略，+号可以省略，(+86)可以省略，11位手机号前的0可以省略；11位手机号第二位数可以是3、4、5、8中的任意一个]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsMobilePhoneNumber(string input)
        {
            string pattern = @"^((\+)?86|((\+)?86)?)0?1[3458]\d{9}$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证电话号码（可以是固定电话号码或手机号码）  
        /// [固定电话：[3位或4位区号；区号可以用小括号括起来；区号可以省略；区号与本地号间可以用减号或空格隔开；可以有3位数的分机号，分机号前要加减号]]  
        /// [手机号码：[可匹配"(+86)013325656352"，括号可以省略，+号可以省略，(+86)可以省略，手机号前的0可以省略；手机号第二位数可以是3、4、5、8中的任意一个]]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsPhoneNumber(string input)
        {
            string pattern = @"^((\+)?86|((\+)?86)?)0?1[3458]\d{9}$|^(((0\d2|0\d{2})[- ]?)?\d{8}|((0\d3|0\d{3})[- ]?)?\d{7})(-\d{3})?$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证邮政编码  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsZipCode(string input)
        {
            //string pattern = @"^\d{6}$";  
            //return IsMatch(input, pattern);  
            if (input.Length != 6)
                return false;
            int i;
            if (int.TryParse(input, out i))
                return true;
            else
                return false;
        }

        /// <summary>  
        /// 验证电子邮箱  
        /// [@字符前可以包含字母、数字、下划线和点号；@字符后可以包含字母、数字、下划线和点号；@字符后至少包含一个点号且点号不能是最后一个字符；最后一个点号后只能是字母或数字]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsEmail(string input)
        {
            ////邮箱名以数字或字母开头；邮箱名可由字母、数字、点号、减号、下划线组成；邮箱名（@前的字符）长度为3～18个字符；邮箱名不能以点号、减号或下划线结尾；不能出现连续两个或两个以上的点号、减号。  
            //string pattern = @"^[a-zA-Z0-9]((?<!(\.\.|--))[a-zA-Z0-9\._-]){1,16}[a-zA-Z0-9]@([0-9a-zA-Z][0-9a-zA-Z-]{0,62}\.)+([0-9a-zA-Z][0-9a-zA-Z-]{0,62})\.?|((25[0-5]|2[0-4]\d|[01]?\d\d?)\.){3}(25[0-5]|2[0-4]\d|[01]?\d\d?)$";  
            string pattern = @"^([\w-\.]+)@([\w-\.]+)(\.[a-zA-Z0-9]+)$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证网址（可以匹配IPv4地址但没对IPv4地址进行格式验证；IPv6暂时没做匹配）  
        /// [允许省略"://"；可以添加端口号；允许层级；允许传参；域名中至少一个点号且此点号前要有内容]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsURL(string input)
        {
            ////每级域名由字母、数字和减号构成（第一个字母不能是减号），不区分大小写，单个域长度不超过63，完整的域名全长不超过256个字符。在DNS系统中，全名是以一个点“.”来结束的，例如“www.nit.edu.cn.”。没有最后的那个点则表示一个相对地址。   
            ////没有例如"http://"的前缀，没有传参的匹配  
            //string pattern = @"^([0-9a-zA-Z][0-9a-zA-Z-]{0,62}\.)+([0-9a-zA-Z][0-9a-zA-Z-]{0,62})\.?$";  

            //string pattern = @"^(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&%_\./-~-]*)?$";  
            string pattern = @"^([a-zA-Z]+://)?([\w-\.]+)(\.[a-zA-Z0-9]+)(:\d{0,5})?/?([\w-/]*)\.?([a-zA-Z]*)\??(([\w-]*=[\w%]*&?)*)$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证IPv4地址  
        /// [第一位和最后一位数字不能是0或255；允许用0补位]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsIPv4(string input)
        {
            //string pattern = @"^(25[0-4]|2[0-4]\d]|[01]?\d{2}|[1-9])\.(25[0-5]|2[0-4]\d]|[01]?\d?\d)\.(25[0-5]|2[0-4]\d]|[01]?\d?\d)\.(25[0-4]|2[0-4]\d]|[01]?\d{2}|[1-9])$";  
            //return IsMatch(input, pattern);  
            string[] IPs = input.Split('.');
            if (IPs.Length != 4)
                return false;
            int n = -1;
            for (int i = 0; i < IPs.Length; i++)
            {
                if (i == 0 || i == 3)
                {
                    if (int.TryParse(IPs[i], out n) && n > 0 && n < 255)
                        continue;
                    else
                        return false;
                }
                else
                {
                    if (int.TryParse(IPs[i], out n) && n >= 0 && n <= 255)
                        continue;
                    else
                        return false;
                }
            }
            return true;
        }

        /// <summary>  
        /// 验证IPv6地址  
        /// [可用于匹配任何一个合法的IPv6地址]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsIPv6(string input)
        {
            string pattern = @"^\s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:)))(%.+)?\s*$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 身份证上数字对应的地址  
        /// </summary>  
        //enum IDAddress  
        //{  
        //    北京 = 11, 天津 = 12, 河北 = 13, 山西 = 14, 内蒙古 = 15, 辽宁 = 21, 吉林 = 22, 黑龙江 = 23, 上海 = 31, 江苏 = 32, 浙江 = 33,  
        //    安徽 = 34, 福建 = 35, 江西 = 36, 山东 = 37, 河南 = 41, 湖北 = 42, 湖南 = 43, 广东 = 44, 广西 = 45, 海南 = 46, 重庆 = 50, 四川 = 51,  
        //    贵州 = 52, 云南 = 53, 西藏 = 54, 陕西 = 61, 甘肃 = 62, 青海 = 63, 宁夏 = 64, 新疆 = 65, 台湾 = 71, 香港 = 81, 澳门 = 82, 国外 = 91  
        //}  

        /// <summary>  
        /// 验证一代身份证号（15位数）  
        /// [长度为15位的数字；匹配对应省份地址；生日能正确匹配]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsIDCard15(string input)
        {
            //验证是否可以转换为15位整数  
            long l = 0;
            if (!long.TryParse(input, out l) || l.ToString().Length != 15)
            {
                return false;
            }
            //验证省份是否匹配  
            //1~6位为地区代码，其中1、2位数为各省级政府的代码，3、4位数为地、市级政府的代码，5、6位数为县、区级政府代码。  
            string address = "11,12,13,14,15,21,22,23,31,32,33,34,35,36,37,41,42,43,44,45,46,50,51,52,53,54,61,62,63,64,65,71,81,82,91,";
            if (!address.Contains(input.Remove(2) + ","))
            {
                return false;
            }
            //验证生日是否匹配  
            string birthdate = input.Substring(6, 6).Insert(4, "/").Insert(2, "/");
            DateTime dt;
            if (!DateTime.TryParse(birthdate, out dt))
            {
                return false;
            }
            return true;
        }

        /// <summary>  
        /// 验证二代身份证号（18位数，GB11643-1999标准）  
        /// [长度为18位；前17位为数字，最后一位(校验码)可以为大小写x；匹配对应省份地址；生日能正确匹配；校验码能正确匹配]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsIDCard18(string input)
        {
            //验证是否可以转换为正确的整数  
            long l = 0;
            if (!long.TryParse(input.Remove(17), out l) || l.ToString().Length != 17 || !long.TryParse(input.Replace('x', '0').Replace('X', '0'), out l))
            {
                return false;
            }
            //验证省份是否匹配  
            //1~6位为地区代码，其中1、2位数为各省级政府的代码，3、4位数为地、市级政府的代码，5、6位数为县、区级政府代码。  
            string address = "11,12,13,14,15,21,22,23,31,32,33,34,35,36,37,41,42,43,44,45,46,50,51,52,53,54,61,62,63,64,65,71,81,82,91,";
            if (!address.Contains(input.Remove(2) + ","))
            {
                return false;
            }
            //验证生日是否匹配  
            string birthdate = input.Substring(6, 8).Insert(6, "/").Insert(4, "/");
            DateTime dt;
            if (!DateTime.TryParse(birthdate, out dt))
            {
                return false;
            }
            //校验码验证  
            //校验码：  
            //（1）十七位数字本体码加权求和公式   
            //S = Sum(Ai * Wi), i = 0, ... , 16 ，先对前17位数字的权求和   
            //Ai:表示第i位置上的身份证号码数字值   
            //Wi:表示第i位置上的加权因子   
            //Wi: 7 9 10 5 8 4 2 1 6 3 7 9 10 5 8 4 2   
            //（2）计算模   
            //Y = mod(S, 11)   
            //（3）通过模得到对应的校验码   
            //Y: 0 1 2 3 4 5 6 7 8 9 10   
            //校验码: 1 0 X 9 8 7 6 5 4 3 2   
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = input.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != input.Substring(17, 1).ToLower())
            {
                return false;
            }
            return true;
        }

        /// <summary>  
        /// 验证身份证号（不区分一二代身份证号）  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsIDCard(string input)
        {
            if (input.Length == 18)
                return IsIDCard18(input);
            else if (input.Length == 15)
                return IsIDCard15(input);
            else
                return false;
        }

        /// <summary>  
        /// 验证经度  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsLongitude(string input)
        {
            ////范围为-180～180，小数位数必须是1到5位  
            //string pattern = @"^[-\+]?((1[0-7]\d{1}|0?\d{1,2})\.\d{1,5}|180\.0{1,5})$";  
            //return IsMatch(input, pattern);  
            float lon;
            if (float.TryParse(input, out lon) && lon >= -180 && lon <= 180)
                return true;
            else
                return false;
        }

        /// <summary>  
        /// 验证纬度  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsLatitude(string input)
        {
            ////范围为-90～90，小数位数必须是1到5位  
            //string pattern = @"^[-\+]?([0-8]?\d{1}\.\d{1,5}|90\.0{1,5})$";  
            //return IsMatch(input, pattern);  
            float lat;
            if (float.TryParse(input, out lat) && lat >= -90 && lat <= 90)
                return true;
            else
                return false;
        }
        #endregion
    }
    /// <summary>
    /// 查询订单状态，返回结果实体
    /// </summary>
    public class OrderQueryRes : OrderInfo
    {
        public string trade_state { get; set; }
        /// <summary>
        /// 交易状态描述 
        /// </summary>
        public string trade_state_desc { get; set; }
    }
    /// <summary>
    /// 微信支付接口请求实体。 包括查询、撤销
    /// </summary>
    public class QueryOrder : BasePay
    {
        /// <summary>
        /// 微信的订单号，优先使用
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>
        /// 商户系统内部的订单号,transaction_id、 out_trade_no 二选一，如果同时存在优先级：transaction_id> out_trade_no
        /// </summary>
        public string out_trade_no { get; set; }
        public string sign { get; set; }
    }
    /// <summary>
    /// 授权类型
    /// </summary>
    public enum AuthType
    {
        /// <summary>
        /// 静默授权，获取openid
        /// </summary>
        snsapi_base,
        /// <summary>
        /// 非静默授权，需用户同意。  获取用户详细信息。
        /// </summary>
        snsapi_userinfo
    }
    /// <summary>
    /// 工具类
    /// </summary>
    public class Utils
    {
        /// <summary>
        ///通过sql字符串获取实体对象
        /// </summary>
        /// <typeparam name="T">实体的类名</typeparam>
        /// <param name="sql">sql查询字符串</param>
        /// <param name="lianjie">数据库的连接字符串</param>
        /// <returns>查询到的实体对象</returns>
        public static List<T> SqlstrToModels<T>(string sql, string lianjie)
        {
            List<T> models = new List<T>();

            SqlConnection con = new SqlConnection(lianjie);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = sql;
            SqlDataReader mydr = cmd.ExecuteReader();
            while(mydr.Read())
            {
                models.Add(ReaderToModel<T>(mydr));
            }
            mydr.Close();
            con.Close();
            cmd.Dispose();
            return models;
        }




        /// <summary>
        ///   将SqlDataReader转换为Model实体
        /// </summary>
        /// <typeparam name="T">实例类名</typeparam>
        /// <param name="dr">Reader对象</param>
        /// <returns>实体对象</returns>
        public static T ReaderToModel<T>(IDataReader dr)
        {
            try
            {

                Type modelType = typeof(T);
                T model = Activator.CreateInstance<T>();
                for (int i = 0; i < dr.FieldCount; i++)
                {
                    if (!IsNullOrDbNull(dr[i]))
                    {
                        PropertyInfo pi = modelType.GetProperty("_" + dr.GetName(i));
                        pi.SetValue(model, HackType(dr[i], pi.PropertyType), null);
                    }
                }
                return model;



            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        ///  对可空类型进行判断.
        /// </summary> 
        private static object HackType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }
                System.ComponentModel.NullableConverter nullAbleConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullAbleConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, conversionType);
        }

        /// <summary>
        ///  判断字段值是否为NUll
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static bool IsNullOrDbNull(object obj)
        {
            return ((obj is DBNull) || string.IsNullOrEmpty(obj.ToString())) ? true : false;
        }

        /// <summary>
        ///  获取属性类的名称
        /// </summary>
        /// <param name="column">列名</param>
        /// <returns>列名</returns>
        private static string GetPropertyName(string column)
        {
            string[] narr = column.Split('_');
            column = "";
            for (int i = 0; i < narr.Length; i++)
            {
                if (narr[i].Length > 1)
                {
                    column += narr[i].Substring(0, 1).ToUpper() + narr[i].Substring(1);
                }
                else
                {
                    column += narr[i].Substring(0, 1).ToUpper();
                }
            }
            return column;
        }



        /// <summary>
        /// 获取当前请请求的URL
        /// </summary>
        /// <returns></returns>
        public static string GetRequestUrl()
        {
            return HttpContext.Current.Request.Url.AbsoluteUri;
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static int GetTimeStamp()
        {
            return ConvertDateTimeInt(DateTime.Now);
        }
        public static T PostResult<T>(Stream stream, string url)
        {
            var retdata = HttpPost(url, stream);
            return JsonConvert.DeserializeObject<T>(retdata);
        }


        /// <summary>
        /// 发起POST请求，并获取请求返回值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="obj">数据实体</param>
        /// <param name="url">接口地址</param>
        public static T PostResult<T>(object obj, string url)
        {
            //序列化设置
            var setting = new JsonSerializerSettings();
            //解决枚举类型序列化时，被转换成数字的问题
            setting.Converters.Add(new StringEnumConverter());
            var retdata = HttpPost(url, JsonConvert.SerializeObject(obj, setting));
            return JsonConvert.DeserializeObject<T>(retdata);
        }
        /// <summary>
        /// 以字符串为分割符，分割字符串
        /// </summary>
        /// <param name="str1">待分割的字符串</param>
        /// <param name="str2">分割符</param>
        /// <returns></returns>
        //public static List<string> FenGe(string str1,string str2)
        //{
        //    List<string> fanhui = new List<string>();

        //}

        /// <summary>
        /// 去除string中的转义字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string quzhuanyi(string str)
        {
            str = str.Replace("'", "''");
            
            return str;
        }

        /// <summary>
        /// 获取数据库中的数据
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        public static string chuli(object z)
        {
            if (z == null)
                return "";
            else
                return z.ToString().Trim();
        }

        /// <summary>
        /// 获取工作表的单元格的值
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string GetDanyuangetext(ExcelWorksheet sheet, int row, int col)
        {
            if (sheet.Cells[row, col].Value == null)
                return "";
            else
                return sheet.Cells[row, col].Value.ToString().Trim();
        }
        /// <summary>
        /// 根据路径删除文件
        /// </summary>
        /// <param name="path"></param>
        public static Boolean DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                FileAttributes attr = File.GetAttributes(path);
                if (attr == FileAttributes.Directory)
                {
                    Directory.Delete(path, true);
                    return true;
                }
                else
                {
                    File.Delete(path);
                    return true;
                }
            }
            else
                return true;
        }
        private static bool SaveBinaryFile(WebResponse response, string savePath)
        {
            bool value = false;
            byte[] buffer = new byte[1024];
            Stream outStream = null;
            Stream inStream = null;
            try
            {
                if (File.Exists(savePath)) File.Delete(savePath);
                outStream = System.IO.File.Create(savePath);
                inStream = response.GetResponseStream();
                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0) outStream.Write(buffer, 0, l);
                } while (l > 0);
                value = true;
            }
            finally
            {
                if (outStream != null) outStream.Close();
                if (inStream != null) inStream.Close();
            }
            return value;
        }
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="picUrl">图片Http地址</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="timeOut">Request最大请求时间，如果为-1则无限制,单位为毫秒</param>
        /// <returns></returns>
        public static bool DownloadPicture(string picUrl, string savePath, int timeOut)
        {
            bool value = false;
            WebResponse response = null;
            Stream stream = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(picUrl);
                if (timeOut != -1)
                    request.Timeout = timeOut;
                response = request.GetResponse();
                stream = response.GetResponseStream();
                if (!response.ContentType.ToLower().StartsWith("text/"))
                    value = SaveBinaryFile(response, savePath);
            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                if (stream != null) stream.Close();
                if (response != null) response.Close();
            }
            finally
            {
                if (stream != null) stream.Close();
                if (response != null) response.Close();
            }
            return value;
        }
        /// <summary>
        /// datetime转换为unixtime
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="url">请求的url</param>
        /// <returns>响应信息</returns>
        public static string HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";//设置请求的方法
            request.Accept = "*/*";//设置Accept标头的值
            string responseStr = "";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())//获取响应
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseStr = reader.ReadToEnd();
                }
            }
            return responseStr;
        }
        /// <summary>
        /// 发起Get请求，并获取请求返回值
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="url">接口地址</param>
        public static T GetResult<T>(string url)
        {
            var retdata = HttpGet(url);
            return JsonConvert.DeserializeObject<T>(retdata);
        }
        /// <summary>
        /// URL字符编码
        /// </summary>
        public static string UrlEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            str = str.Replace("'", "");
            return HttpContext.Current.Server.UrlEncode(str);
        }
        /// <summary>
        /// 得到当前完整主机头
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            if (!request.Url.IsDefaultPort)
                return string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());

            return request.Url.Host;
        }
        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;
            if (string.IsNullOrEmpty(result) || !Regex.IsMatch(result, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"))
                return "127.0.0.1";
            return result;
        }
        public static string GetGuid()
        {
            return Guid.NewGuid().ToString("N");
        }
        /// <summary>
        /// 获取当前请求的数据包内容
        /// </summary>
        public static string GetRequestData()
        {
            using (var stream = HttpContext.Current.Request.InputStream)
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        /// <summary>
        /// 将实体对象转换成键值对。移除值为null的属性
        /// </summary>
        /// <param name="obj">参数实体</param>
        /// <returns>数据键值对</returns>
        public static Dictionary<string, string> EntityToDictionary(object obj, int? index = null)
        {
            var type = obj.GetType();
            var dic = new Dictionary<string, string>();
            var pis = type.GetProperties();
            foreach (var pi in pis)
            {
                //获取属性的值
                var val = pi.GetValue(obj, null);
                //移除值为null，以及字符串类型的值为空字符的。另外，签名字符串本身不参与签名，
                //在验证签名正确性时，需移除sign
                if (val == null || val.ToString() == "" || pi.Name == "sign" ||
              pi.PropertyType.IsGenericType)
                    continue;
                if (index != null)
                {
                    dic.Add(pi.Name + "_" + index, val.ToString());
                }
                else
                {
                    dic.Add(pi.Name, val.ToString());
                }
            }
            var classlist = pis.Where(p => p.PropertyType.IsGenericType);
            foreach (var info in classlist)
            {
                var val = info.GetValue(obj, null);
                if (val == null)
                {
                    continue;
                }
                int count = (int)info.PropertyType.GetProperty("Count").GetValue(val, null);
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        object ol = info.PropertyType.GetMethod("get_Item").Invoke
                        (val, new object[] { i });
                        var tem = EntityToDictionary(ol, i);//递归调用
                        foreach (var t in tem)
                        {
                            dic.Add(t.Key, t.Value);
                        }
                    }
                }
            }
            return dic;
        }
        /// <summary>
        /// 获取支付签名
        /// </summary>
        /// <param name="dictionary">数据集合</param>
        /// <param name="key">支付密钥</param>
        /// <returns>签名</returns>
        public static string GetPaySign(Dictionary<string, string> dictionary, string key)
        {
            var arr = dictionary.OrderBy(d => d.Key).Select(d => string.Format("{0}={1}", d.Key, d.Value)).ToArray();
            string stringA = string.Join("&", arr) + "&key=" + key;
            return MD5(stringA).ToUpper();
        }
        /// <summary>
        /// 获取支付签名
        /// </summary>
        /// <param name="obj">对象实体</param>
        /// <param name="key">支付密钥</param>
        /// <returns></returns>
        public static string GetPaySign(object obj, string key)
        {
            var dic = EntityToDictionary(obj);
            return GetPaySign(dic, key);
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="pwd">要加密的字符串</param>
        /// <param name="encoding">字符编码方法。默认utf-8</param>
        /// <returns>加密后的密文</returns>
        public static string MD5(string pwd, string encoding = "utf-8")
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = Encoding.GetEncoding(encoding).GetBytes(pwd);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();
            string str = "";
            for (int i = 0; i < md5data.Length; i++)
            {
                str += md5data[i].ToString("x").PadLeft(2, '0');
            }
            return str;
        }
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="obj">参数集实体</param>
        /// <param name="sign">签名</param>
        /// <param name="key">支付密钥</param>
        /// <returns></returns>
        public static bool ValidSign(object obj, string sign, string key)
        {
            var tempsign = GetPaySign(obj, key);
            return tempsign == sign;
        }
        public static string parseXML(Dictionary<string, string> parameters)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            var arr = parameters.Select(p => string.Format(Regex.IsMatch(p.Value, @"^[0-9.]$") ? "<{0}>{1}</{0}>" : "<{0}><![CDATA[{1}]]></{0}>", p.Key, p.Value));
            sb.Append(string.Join("", arr));
            sb.Append("</xml>");
            return sb.ToString();
        }

        /// <summary>
        /// 将XML字符串转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T XmlToEntity<T>(string xml)
        {

            var type = typeof(T);
            //创建实例
            var t = Activator.CreateInstance<T>();
            var pr = type.GetProperties();
            var xdoc = XElement.Parse(xml);
            var eles = xdoc.Elements();
            var ele = eles.Where(e => new Regex(@"_\d{1,}$").IsMatch(e.Name.ToString()));//获取带下标的节点
            if (ele.Count() > 0)
            {
                var selele = ele.Select(e =>
                {
                    var temp = e.Name.ToString().Split('_');
                    var index = int.Parse(temp[temp.Length - 1]);
                    return new { Index = index, Property = e.Name.ToString().Replace("_" + index.ToString(), ""), Value = e.Value };
                });//转换为方便操作的匿名对象。
                var max = selele.Max(m => m.Index);//获取最大索引的值
                var infos = pr.FirstOrDefault(f => f.PropertyType.IsGenericType);//获取类型为泛型的属性
                if (infos != null)
                {
                    var infotype = infos.PropertyType.GetGenericArguments().First();//获取泛型的真实类型
                    Type listType = typeof(List<>).MakeGenericType(new[] { infotype });//创建泛型列表
                    var datalist = Activator.CreateInstance(listType);//创建对象
                    var infoprs = infotype.GetProperties();
                    for (int j = 0; j <= max; j++)
                    {
                        var temp = Activator.CreateInstance(infotype);
                        var list = selele.Where(s => s.Index == 0);
                        foreach (var v in list)
                        {
                            var p = infoprs.FirstOrDefault(f => f.Name == v.Property);
                            if (p == null) continue;
                            p.SetValue(temp, v.Value, null);
                        }
                        listType.GetMethod("Add").Invoke((object)datalist, new[] { temp });//将对象添加到列表中
                    }
                    infos.SetValue(t, datalist, null);//最后给泛型属性赋值
                }
                ele.Remove();//将有下标的节点从集合中移除
            }
            foreach (var element in eles)
            {
                var p = pr.First(f => f.Name == element.Name);
                p.SetValue(t, Convert.ChangeType(element.Value, p.PropertyType), null);
            }

            return t;
        }
        /// <summary>
        /// 带验证证书的POST请求
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="data">请求数据</param>
        /// <param name="certpath">证书路径</param>
        /// <param name="certpwd">证书密码</param>
        public static string HttpPost(string url, string data, string certpath = "", string certpwd = "")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //当请求为https时，验证服务器证书
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((a, b, c, d) =>
            {
                if (d == SslPolicyErrors.None)
                    return true;
                return false;
            });
            if (!string.IsNullOrEmpty(certpath) && !string.IsNullOrEmpty(certpwd))
            {
                X509Certificate2 cer = new X509Certificate2(certpath, certpwd,
                    X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
                request.ClientCertificates.Add(cer);
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;
            string responseStr = "";
            using (StreamWriter requestStream = new StreamWriter(request.GetRequestStream()))
            {
                requestStream.Write(data);//将请求的数据写入到请求流中
            }
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseStr = reader.ReadToEnd();//获取响应
                        Utils.WriteTxt("/debug.txt", responseStr);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return responseStr;
        }

        public static string HttpPost(string url, Stream stream)
        {
            //当请求为https时，验证服务器证书
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((a, b, c, d) => true);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;
            string responseStr = "";
            using (var reqstream = request.GetRequestStream())
            {
                stream.Position = 0L;
                stream.CopyTo(reqstream);
            }
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseStr = reader.ReadToEnd();//获取响应
                }
            }
            return responseStr;
        }

        public static void WriteTxt(string path, string txt)
        {
            using (FileStream fs = new FileStream(HttpContext.Current.Request.MapPath(path), FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(txt);
                    sw.Flush();
                }
            }
        }

    }

    public class WxBaseConfig
    {
        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        public const string APPID = "wxbd539b686b0d3597";
        public const string APPSECRET = "59739fceb0578ac9777ba3f690188134";

    }

    public class WxPayConfig : WxBaseConfig
    {
        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */

        public const string MCHID = "1512765681";
        public const string KEY = "Qsej8374df8efkeBufJI943zfdSf83Nf";


        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public const string SSLCERT_PATH = "cert/apiclient_cert.p12";
        public const string SSLCERT_PASSWORD = "1233410002";



        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */
        public const string NOTIFY_URL = "http://paysdk.weixin.qq.com/example/ResultNotifyPage.aspx";

        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        public const string IP = "8.8.8.8";


        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        public const string PROXY_URL = "http://10.152.18.220:8080";

        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public const int REPORT_LEVENL = 1;

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        public const int LOG_LEVENL = 0;

    }

    public class Pay
    {
        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="orderQuery">订单查询实体</param>
        /// <param name="key">支付密钥</param>
        /// <returns></returns>
        public static OrderQueryRes QueryOrder(QueryOrder orderQuery, string key)
        {
            var url = "https://api.mch.weixin.qq.com/pay/orderquery";
            return PayRequest<OrderQueryRes>(orderQuery, key, url);
        }
        public static void BackMessage(string msg = "")
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (msg != "")
            {
                dic.Add("return_code", "FAIL");
                dic.Add("return_msg", msg);
                HttpContext.Current.Response.Write(Utils.parseXML(dic));
            }
            else
            {
                dic.Add("return_code", "SUCCESS");
                HttpContext.Current.Response.Write(Utils.parseXML(dic));
            }
        }
        /// <summary>
        /// 获取通用回调
        /// </summary>
        /// <param name="key"></param>
        /// <param name="callBack"></param>
        public static void GetNotifyRes(string key, Action<OrderInfo> callBack)
        {
            try
            {
                var reqdata = Utils.GetRequestData();
                var rev = Utils.XmlToEntity<OrderInfo>(reqdata);
                if (rev.return_code != "SUCCESS")
                { BackMessage("通讯错误"); return; }
                if (rev.result_code != "SUCCESS")
                { BackMessage("业务出错"); return; }
                if (rev.sign == Utils.GetPaySign(rev, key))
                {
                    //回调函数，业务逻辑处理，处理结束后返回信息给微信
                    callBack(rev);
                }
            }
            catch (Exception e)
            {
                BackMessage("回调函数处理错误");
            }
        }
        /// <summary>
        /// 调用统一下单接口并解析返回的XML
        /// </summary>
        /// <returns>响应实体</returns>
        public static UnifiedRes UnifiedOrder(UnifiedEntity entity, string key)
        {
            var url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            return PayRequest<UnifiedRes>(entity, key, url);
        }
        /// <summary>
        /// 微信支付接口请求
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="obj">请求实体</param>
        /// <param name="key">支付密钥</param>
        /// <param name="url">接口地址</param>
        /// <param name="certpath">证书路径，为null时说明不适用证书</param>
        /// <param name="certpwd">证书密码</param>
        public static T PayRequest<T>(object obj, string key, string url, string certpath = "", string certpwd = "")
        {
            var param = Utils.EntityToDictionary(obj);//将实体转换成数据集合
            param.Add("sign", Utils.GetPaySign(param, key));//生成签名，并将签名添加到数据集合
            var xml = Utils.parseXML(param);//将数据集合转换成xml
            return Utils.XmlToEntity<T>(Utils.HttpPost(url, xml, certpath, certpwd));
        }


    }

    /// <summary>
    ///微信支付基类
    /// </summary>
    public abstract class BasePay
    {
        /// <summary>
        /// 微信分配的公众账号ID
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 微信支付分配的商户号
        /// </summary>
        public string mch_id { get; set; }
        /// <summary>
        /// 随机字符串，不长于32位
        /// </summary>
        public string nonce_str { get; set; }
    }
    /// <summary>
    /// 统一支付接口请求实体
    /// </summary>
    public class UnifiedEntity : BasePay
    {
        /// <summary>
        /// 微信支付分配的终端设备号
        /// </summary>
        public string device_info { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 商品描述最大长度127
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 商品详情
        /// </summary>
        public string detail { get; set; }
        /// <summary>
        /// 附加数据，原样返回
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// 商户系统内部的订单号,32 个字符内、可包含字母,确保在商户系统唯一,详细说明
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 货币类型 
        /// </summary>
        public string fee_type { get; set; }
        /// <summary>
        /// 订单总金额，单位为分，不能带小数点
        /// </summary>
        public int total_fee { get; set; }
        /// <summary>
        /// 终端IP
        /// </summary>
        public string spbill_create_ip { get; set; }
        /// <summary>
        /// 交易起始时间
        /// </summary>
        public string time_start { get; set; }
        /// <summary>
        /// 交易结束时间
        /// </summary>
        public string time_expire { get; set; }
        /// <summary>
        /// 接收微信支付成功通知
        /// </summary>
        public string notify_url { get; set; }
        /// <summary>
        /// JSAPI、NATIVE、APP
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// 用户在商户appid下的唯一标识，trade_type为JSAPI 时，此参数必传
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 只在 trade_type 为 NATIVE 时需要填写。此id为二维码中包含的商品ID，商户自行维护。
        /// </summary>
        public string product_id { get; set; }
    }
    /// <summary>
    /// 支付接口调用后，响应参数基础类。
    /// </summary>
    public class BasePayRes : BasePay
    {
        /// <summary>
        /// SUCCESS/FAIL此字段是通信标识，非交易标识，交易是否成功需要查看result_code来判断
        /// </summary>
        public string return_code { get; set; }
        /// <summary>
        /// 返回信息，如非空，为错误原因
        /// </summary>
        public string return_msg { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 业务结果
        /// </summary>
        public string result_code { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        public string err_code { get; set; }
        /// <summary>
        /// 错误代码描述 
        /// </summary>
        public string err_code_des { get; set; }
    }
    /// <summary>
    /// 调用统一支付接口后，返回的实体
    /// </summary>
    public class UnifiedRes : BasePayRes
    {
        /// <summary>
        /// 设备号
        /// </summary>
        public string device_info { get; set; }
        /// <summary>
        /// 预支付ID
        /// </summary>
        public string prepay_id { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// 二维码链接
        /// </summary>
        public string code_url { get; set; }
    }

    public class OrderInfo : BasePayRes
    {

        public string openid { get; set; }
        /// <summary>
        /// 是否订阅
        /// </summary>
        public string is_subscribe { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// 付款银行
        /// </summary>
        public string bank_type { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public int total_fee { get; set; }
        /// <summary>
        /// 代金券或立减优惠金额 
        /// </summary>
        public string coupon_fee { get; set; }
        /// <summary>
        /// 代金券或立减优惠使用数量 
        /// </summary>
        public string coupon_count { get; set; }

        /// <summary>
        /// 货币种类
        /// </summary>
        public string fee_type { get; set; }
        /// <summary>
        /// 微信支付订单号
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 商家数据包
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// 支付完成时间
        /// </summary>
        public string time_end { get; set; }
        /// <summary>
        /// 现金支付金额 
        /// </summary>
        public int cash_fee { get; set; }

        public string cash_fee_type { get; set; }
        public List<CouponInfo> CouponInfo { get; set; }
    }

    /// <summary>
    /// 代金券或立减优惠实体
    /// </summary>
    public class CouponInfo
    {
        /// <summary>
        /// 代金券或立减优惠批次ID
        /// </summary>
        public string coupon_batch_id { get; set; }
        /// <summary>
        ///代金券或立减优惠ID
        /// </summary>
        public string coupon_id { get; set; }
        /// <summary>
        /// 单个代金券或立减优惠支付金额
        /// </summary>
        public int coupon_fee { get; set; }
    }
    /// <summary>
    /// OAuth授权类
    /// </summary>
    public class OAuth
    {
        /// <summary>
        /// 构造授权链接
        /// </summary>
        public static string GetAuthUrl(string appid, string redirect_uri, string state, AuthType authType = AuthType.snsapi_base)
        {
            var url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}#wechat_redirect";
            return string.Format(url, appid, Utils.UrlEncode(redirect_uri), authType, state);
        }
        /// <summary>
        /// 获取OAuthToken
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static OAuthToken GetAuthToken(string appid, string secret, string code)
        {
            var url =
                string.Format(
                    "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code"
                    , appid, secret, code);
            return Utils.GetResult<OAuthToken>(url);
        }
        /// <summary>
        /// 刷新OAuthToken
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="refresh_token"></param>
        /// <returns></returns>
        public static OAuthToken GetRefreshToken(string appid, string refresh_token)
        {
            var url =
                string.Format(
                    "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}", appid, refresh_token);

            return Utils.GetResult<OAuthToken>(url);
        }
        /// <summary>
        /// 拉取用户信息(需scope为 snsapi_userinfo)
        /// </summary>
        /// <param name="authtoken"></param>
        /// <param name="openid"></param>
        /// <returns></returns>

        public static UserInfo GetUserInfo(string authtoken, string openid)
        {
            var url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", authtoken, openid);
            return Utils.GetResult<UserInfo>(url);
        }
    }

    /// <summary>
    /// 网页授权获取access_token时返回的实体
    /// </summary>
    public class OAuthToken : ErrorEntity
    {
        /// <summary>
        /// 网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
        /// </summary>
        public string access_token { get; set; }

        private int _expires_in;
        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        public int expires_in
        {
            get { return _expires_in; }
            set
            {
                expires_time = DateTime.Now.AddSeconds(value);
                _expires_in = value;
            }
        }
        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// 用户唯一标识，请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        public string scope { get; set; }
        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。
        /// </summary>
        public string unionid { get; set; }

        public DateTime expires_time { get; set; }
    }

    /// <summary>
    /// 错误信息实体
    /// </summary>
    public class ErrorEntity
    {
        private int _errCode { get; set; }
        /// <summary>
        /// 错误编码
        /// </summary>
        public int ErrCode
        {
            get { return _errCode; }
            set
            {
                _errCode = value;
                //根据错误码，从错误列表中找到错误信息，并给ErrDescription赋值
                ErrDescription = ErrList.FirstOrDefault(e => e.Key == value).Value;
            }
        }
        /// <summary>
        /// 错误描述
        /// </summary>
        public string ErrDescription { get; set; }

        private static Dictionary<int, string> _errorDic;

        public static Dictionary<int, string> ErrList
        {
            get
            {
                if (_errorDic != null && _errorDic.Count > 0)
                    return _errorDic;
                _errorDic = new Dictionary<int, string>();
                //var temp = Resources.Resource.CodeInfo.Split(new char[] { '\r', '\n' },
                //    StringSplitOptions.RemoveEmptyEntries);
                //foreach (var strArr in temp.Select(str => str.Split(new char[] { '\t', ' ' },
                //    StringSplitOptions.RemoveEmptyEntries)))
                //{
                //    _errorDic.Add(int.Parse(strArr[0]), strArr[1]);
                //}
                return _errorDic;
            }
        }
    }


    public class UserInfo : ErrorEntity
    {
        /// <summary>
        /// 是否关注
        /// </summary>
        public int subscribe { get; set; }
        public string openid { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int sex { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public string language { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 广东
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 中国
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 图像
        /// </summary>
        public string headimgurl { get; set; }
        /// <summary>
        /// 关注时间
        /// </summary>
        public int subscribe_time { get; set; }

        public string unionid { get; set; }
        public string remark { get; set; }
        public string[] privilege { get; set; }
    }
    public class JsEntity
    {
        /// <summary>
        /// 公众号id
        /// </summary>
        public string appId { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string timeStamp { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonceStr { get; set; }
        /// <summary>
        /// 订单详情扩展字符串
        /// </summary>
        public string package { get; set; }
        /// <summary>
        /// 签名类型
        /// </summary>
        public string signType { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string paySign { get; set; }
    }
    public class JsApiTicket : ErrorEntity
    {
        /// <summary>
        /// ticket
        /// </summary>
        public string ticket { get; set; }
        private int _expires_in;
        /// <summary>
        /// 有效期时间。单位为秒
        /// </summary>
        public int expires_in
        {
            get { return _expires_in; }
            set
            {
                //获取失效时间
                expires_time = DateTime.Now.AddSeconds(value);
                _expires_in = value;
            }
        }
        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime expires_time { get; set; }
    }
    /// <summary>
    /// accesstoken箱子类。支持多微信。
    /// </summary>
    public class AccessTokenBox
    {
        public string AppId { get; set; }
        public AccessToken Token { get; set; }
        private static List<AccessTokenBox> _boxs;
        private static object lockobj = new object();
        public static string GetTokenValue(string appid, string appSecret)
        {
            lock (lockobj)
            {
                _boxs = (_boxs == null ? new List<AccessTokenBox>() :
                _boxs.Where(b => b.Token.ExpirationTime > DateTime.Now).ToList());
                var tempat = _boxs.FirstOrDefault(b => b.AppId == appid);
                if (tempat != null)
                {
                    return tempat.Token.access_token;
                }
                var newAT = BaseServices.GetAccessToken(appid, appSecret);
                if (!string.IsNullOrEmpty(newAT.access_token))
                {
                    _boxs.Add(new AccessTokenBox
                    {
                        AppId = appid,
                        Token = newAT
                    });
                    return newAT.access_token;
                }
                else
                {
                    //此处可以写日志，保存错误信息
                    return "";
                }
            }

        }
    }
    /// <summary>
    /// 全局票据实体
    /// </summary>
    public class AccessToken : ErrorEntity
    {
        /// <summary>
        /// AccessToken的值
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpirationTime { get; set; }
        private int _expires_in;
        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public int expires_in
        {
            set
            {
                ExpirationTime = DateTime.Now.AddSeconds(value / 2);
                _expires_in = value;
            }
        }
    }
    public class BaseServices
    {
        /// <summary>
        /// 接入验证URL
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool ValidUrl(string token)
        {
            //获取参与校验的参数
            var signature = HttpContext.Current.Request.QueryString["signature"];
            var timestamp = HttpContext.Current.Request.QueryString["timestamp"];
            var nonce = HttpContext.Current.Request.QueryString["nonce"];
            string[] temp = { token, timestamp, nonce };
            Array.Sort(temp);//字典序排序
            var tempstr = string.Join("", temp);//拼接字符串 
                                                // SHA1加密
            var tempsign = FormsAuthentication.HashPasswordForStoringInConfigFile(tempstr, "SHA1").ToLower();
            if (tempsign == signature)
            {
                var echostr = HttpContext.Current.Request.QueryString["echostr"];
                HttpContext.Current.Response.Write(echostr);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取access_token
        /// </summary>
        /// <param name="appid">应用ID</param>
        /// <param name="secret">应用密钥</param>
        /// <returns>AccessToken实体</returns>
        public static AccessToken GetAccessToken(string appid, string secret)
        {
            var url =
            string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);
            return Utils.GetResult<AccessToken>(url);
        }
        /// <summary>
        /// 获取微信服务器的IP列表
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static IpEntity GetIpArray(string accessToken)
        {
            var url =
            string.Format("https://api.weixin.qq.com/cgi-bin/getcallbackip?access_token={0}", accessToken);
            return Utils.GetResult<IpEntity>(url);
        }

        /// <summary>
        /// 将一条长链接转成短链接
        /// </summary>
        /// <param name="longurl">长链接</param>
        /// <param name="accessToken">accessToken</param>
        /// <returns>包含短连接和错误代码的实体</returns>
        public static ShortUrl LongUrlToShortUrl(string longurl, string accessToken)
        {
            var url =
         string.Format("https://api.weixin.qq.com/cgi-bin/shorturl?access_token={0}", accessToken);
            var json = new { action = "long2short", long_url = longurl };
            return Utils.PostResult<ShortUrl>(json, url);
        }
        /// <summary>
        /// 获取IP列表接口返回实体
        /// </summary>
        public class IpEntity : ErrorEntity
        {
            /// <summary>
            /// IP列表
            /// </summary>
            public string[] ip_list { get; set; }
        }
        public class ShortUrl : ErrorEntity
        {
            public string short_url { get; set; }
        }

    }

    public class JsApi
    {
        /// <summary>
        /// 获取jssdk Ticket
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static JsApiTicket GetHsJsApiTicket(string accessToken)
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", accessToken);
            return Utils.GetResult<JsApiTicket>(url);
        }
        /// <summary>
        /// 获取jssdk签名
        /// </summary>
        /// <param name="noncestr">随机字符串</param>
        /// <param name="jsapi_ticket">ticket</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="url">当前网页的URL</param>
        /// <returns></returns>
        public static string GetJsApiSign(string noncestr, string jsapi_ticket, string timestamp, string url)
        {
            //将字段添加到列表中
            string[] arr = new[]
            {
                string.Format("noncestr={0}",noncestr),
                string.Format("jsapi_ticket={0}",jsapi_ticket),
                string.Format("timestamp={0}",timestamp),
                string.Format("url={0}",url)
             };
            //字典排序
            Array.Sort(arr);
            //使用URL键值对的格式拼接成字符串
            var temp = string.Join("&", arr);
            return FormsAuthentication.HashPasswordForStoringInConfigFile(temp,
            "SHA1");
        }
        public static WxJsParam GetJsParam(string appId, bool debug)
        {
            var param = new WxJsParam
            {
                appId = appId,
                debug = debug,
                nonceStr = Utils.GetTimeStamp().ToString(),
                timestamp = Utils.GetTimeStamp(),
                url = Utils.GetRequestUrl()
            };
            return param;
        }
    }

    public class WxJsParam
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public int timestamp { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonceStr { get; set; }
        /// <summary>
        /// 当前页面的url
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string signature { get; set; }
        /// <summary>
        /// 是否开启调试模式
        /// </summary>
        public bool debug { get; set; }
        /// <summary>
        /// 公众号ID
        /// </summary>
        public string appId { get; set; }
    }
}