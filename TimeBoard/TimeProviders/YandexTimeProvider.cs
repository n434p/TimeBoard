using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;


namespace TimeBoard.TimeProviders
{
    delegate void TimeProviderEventHandler(List<City> list);
    public class YandexTimeProvider
    {
        const string cityListRequestPrefix = "https://yandex.ua/time/suggest.json?lang=en&part=";
        const string cityDetailsRequesPrefix = "https://yandex.ua/time/sync.json?lang=en&geo=";

        IWebProxy proxy;

        public YandexTimeProvider()
        {
            var addr = "217.182.79.247:3128"; // new PluginProxy().GetProxyAddress();
            if (addr != null)
                proxy = new WebProxy(addr);
            else
                proxy = WebRequest.GetSystemWebProxy();
        }

        public void GetCityList(string searchString, Action<List<City>> callback)
        { 
            List<City> list = null;
            Thread t = new Thread((s) =>
            {
                using (WebClient wc = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    wc.Proxy = proxy;

                    var searchUriString = cityListRequestPrefix + s;
                    try
                    {
                        var str = wc.DownloadString(new Uri(searchUriString));
                        list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<City>>(str).Where(c => !c.id.Contains('/')).ToList();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Yandex List>>>>>" + ex.Message);
                    }
                    finally
                    {
                        if (callback != null)
                            callback(list);
                    }
                }
            });
            t.Start(searchString);
        }
        public void GetCityInfo(string searchString, Action<City> callback)
        {
            City city = null;
            Thread t = new Thread((s) =>
            {
                using (WebClient wc = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    wc.Proxy = proxy;

                    var searchUriString = cityDetailsRequesPrefix + s;
                    try
                    {
                        var str = wc.DownloadString(new Uri(searchUriString));
                        var info = JObject.Parse(str)["clocks"][s];
                        city = info.ToObject<City>();
                        city.Regions = info["parents"].Select(p => (string)p["name"]).ToList<string>();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Yandex Details>>>>>" + ex.Message);
                    }
                    finally
                    {
                        if (callback != null)
                            callback(city);
                    }
                }
            });
            t.Start(searchString);
        }

    }
}
