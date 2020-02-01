using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace douyin
{
    public class douyin
    {
        /// <summary>
        /// 取抖音用户资料
        /// </summary>
        /// <param name="url">用户链接</param>
        /// <returns></returns>
        public UserData UserData(string url)
        {
            UserData userData = new UserData();
            string html = new web().getHtml(url);//取网页源码
            if (html != null)
            {
                userData.Signature = GetData("<p class=\"signature\">", "</p>", html);//个人签名
                userData.Name = GetData("<p class=\"nickname\">", "</p>", html);//网名
                userData.HeadPortrait = GetData("<img class=\"avatar\" src=\"", "\">", html);//头像
                userData.FocusOn = GetData("</p><p class=\"follow-info\"><span class=\"focus block\"><span class=\"num\">    <i class=\"icon iconfont follow-num\">", "关注", html);//取关注                                                                                                                                                                    //关注通过字体反爬处理，需要通过以下步骤进行处理
                userData.FocusOn = GetNumMapping(userData.FocusOn);
                userData.Fans = GetData("</span><span class=\"follower block\"><span class=\"num\">    <i class=\"icon iconfont follow-num\">","粉丝",html);//取粉丝                                                                                                                                                //粉丝也通过字体反爬处理，和上面步骤相同
                userData.Fans = GetNumMapping(userData.Fans);
                userData.Works = GetData("作品", "喜欢", html);
                userData.Works = GetNumMapping(userData.Works);
                userData.Like = GetData("喜欢", "pagelet-worklist", html);
                userData.Like = GetNumMapping(userData.Like);
            }
            return userData;
        }

        internal string GetNumMapping(string str)
        {
            string Num = "";
            string[] DataNum = DataNumSplit(str);//取字体UTF8编码
            foreach (var i in DataNum)//将取出的字体UTF8编码识别成数字
            {
                Num += NumMapping(i);
            }
            return Num;
        }

        /// <summary>
        /// 映射数字
        /// </summary>
        /// <param name="NumUTF8">数字UTF8编码</param>
        /// <returns></returns>
        internal string NumMapping(string NumUTF8)
        {
            NumMapping numMapping = new NumMapping();
            for (int i = 0; i < numMapping.mapcodeToFont.GetLength(0); i++)
            { 
                if(numMapping.mapcodeToFont[i,0] == NumUTF8)
                {
                    for (int t = 0; t < numMapping.mapFontToNum.GetLength(0); t++)
                    {
                        if (numMapping.mapFontToNum[t, 0] == numMapping.mapcodeToFont[i,1])
                        {
                            return numMapping.mapFontToNum[t, 1];
                        }
                    }
                }
            }
            return NumUTF8;
        }

        internal string[] DataNumSplit(string str)
        {
            List<string> Num = new List<string>();
            string[] strArrayA = str.Split(';');
            for (int i = 0; i < strArrayA.Length; i++)
            {
                if (strArrayA[i].IndexOf('.') >= 0)
                {
                    Num.Add(".");
                }
                if (i == strArrayA.Length - 1)
                {
                    if (strArrayA[i].IndexOf('w') >= 0)
                    {
                        Num.Add("w");
                    }
                    else if (strArrayA[i].IndexOf('亿') >= 0)
                    {
                        Num.Add("亿");
                    }
                    break;
                }
                string[] strArrayB = Regex.Split(strArrayA[i],"&#");
                Num.Add("0" + strArrayB[1]);
            }
            return Num.ToArray();
        }

        internal string GetData(string A, string B, string html)
        {
            string[] Array = Regex.Split(html, A);//利用正则分割数组
            return LookingFor(Array[1], B);
        }

        /// <summary>
        /// 寻找指定字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="NeedToFind">需查找字符串</param>
        /// <returns></returns>
        internal string LookingFor(string str,string NeedToFind)
        {
            return Regex.Split(str, NeedToFind)[0];
        }
    }

    /// <summary>
    /// 用户资料
    /// </summary>
    public class UserData
    { 
        /// <summary>
        /// 网名
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadPortrait { set; get; }
        /// <summary>
        /// 签名
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 关注
        /// </summary>
        public string FocusOn { get; set; }
        /// <summary>
        /// 粉丝
        /// </summary>
        public string Fans { get; set; }
        /// <summary>
        /// 作品
        /// </summary>
        public string Works { get; set; }
        /// <summary>
        /// 喜欢
        /// </summary>
        public string Like { get; set; }
    }
}
