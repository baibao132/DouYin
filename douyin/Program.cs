using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace douyin
{
    class Program
    {
        static void Main(string[] args)
        {
            douyin douyin = new douyin();
            UserData ud = douyin.UserData(" https://v.douyin.com/gH762n/");
            Console.WriteLine("头像：{0}\n网名：{1}\n关注数：{2}\n粉丝数：{3}\n作品数:{4}\n喜欢数：{5}\n个性签名：{6}",ud.HeadPortrait,ud.Name,ud.FocusOn,ud.Fans,ud.Works,ud.Like,ud.Signature);
            Console.ReadKey();
        }
    }
}
