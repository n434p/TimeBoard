using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TimeBoard.TimeProviders
{
    class PluginProxy
    {
        string proxyRequest = "https://api.getproxylist.com/proxy?notCountry=UA&minDownloadSpeed=200"; //&maxSecondsToFirstByte=1&maxConnectTime=1";

        public string GetProxyAddress()
        {
            string link = null;

            using (WebClient wc = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    try
                    {
                        var str = wc.DownloadString(new Uri(proxyRequest));
                        link = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomProxy>(str).ToString();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("getproxylist >>>>>" + ex.Message);
                    }
                }

            return link;
        }

        class CustomProxy
        {
            public string ip;
            public string port;

            public override string ToString()
            {
                return string.Format("{0}:{1}", ip, port);
            }
        }
    }
}
