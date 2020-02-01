using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace douyin
{
    internal class web
    {
        public string getHtml(string url)
        {
            Stream stream;
            WebClient client = new WebClient();
            try
            {
                stream = client.OpenRead(url);
                stream.ReadTimeout = 1000;
            }
            catch (WebException exception1)
            {
                string message = exception1.Message;
                return null;
            }
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            try
            {
                return reader.ReadToEnd();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
