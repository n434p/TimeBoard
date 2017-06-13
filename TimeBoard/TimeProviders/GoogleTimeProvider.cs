using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TimeBoard.TimeProviders
{
    public class GoogleTimeProvider : ITimeProvider
    {
        const string placeIdListRequestPrefix = "https://maps.googleapis.com/maps/api/place/autocomplete/json?language=en&types=(cities)&key=AIzaSyAWzDF_-ysN2qURGCCUfk21cXDAweR6As8&input=";
        const string locationRequesPrefix = "https://maps.googleapis.com/maps/api/geocode/json?key=AIzaSyARCqhhsufDuvmIevwja-ZP4-8MsVk8OLE&&address=";
        const string timezoneRequesPrefix = "https://maps.googleapis.com/maps/api/timezone/json?key=AIzaSyBa8PqrzOgYNgC9Chmuv78hgjyuteLi-Us&location=";

        NumberFormatInfo nfi = new NumberFormatInfo() { CurrencyDecimalSeparator = "." };

        public void GetCityInfo(string searchString, Action<City> callback)
        {
            City city = null;
            Thread t = new Thread((s) =>
            {
                using (WebClient wc = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    var name = s.ToString().Replace(" ", "+");
                    var searchUriString = locationRequesPrefix + name;
                    try
                    {
                        var locationString = wc.DownloadString(new Uri(searchUriString));

                        var info = JObject.Parse(locationString)["results"][0]["geometry"]["location"];

                        var id = (string)JObject.Parse(locationString)["results"][0]["place_id"];

                        var loc = string.Join(",", ((double)info["lat"]).ToString(nfi), ((double)info["lng"]).ToString(nfi));

                        var timezoneUri = timezoneRequesPrefix + loc + "&timestamp="+ ConvertToUnixTimestamp(DateTime.UtcNow);

                        var timezoneString = wc.DownloadString(new Uri(timezoneUri));

                        var offset = (int)JObject.Parse(timezoneString)["rawOffset"];

                        city = new City() { id = id, name = (string)s, offset = offset };
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Timeboard Google API Details>>>>>" + ex.Message);
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

        public void GetCityList(string searchString, Action<List<City>> callback)
        {
            List<City> list = null;
            Thread t = new Thread((s) =>
            {
                using (WebClient wc = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    var searchUriString = placeIdListRequestPrefix + s;
                    try
                    {
                        var str = wc.DownloadString(new Uri(searchUriString));
                        var res = JObject.Parse(str);

                        list = res["predictions"].Children().Select(x => new City()
                        {
                            name = x.SelectToken("description").ToString(),
                            id = x.SelectToken("place_id").ToString()
                        }).ToList();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Timeboard Google API Details>>>>>" + ex.Message);
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

        double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
    }
}
