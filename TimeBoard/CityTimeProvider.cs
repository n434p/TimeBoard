using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TimeBoard
{

    public class CityTimeProvider
    {
        const string citiesListRequestPrefix = "http://services.gisgraphy.com/fulltext/suggest?format=json&suggest=true&allwordsrequired=false&style=medium&placetype=city&placetype=adm&from=1&to=8&q=";
        const string timezoneRequesPrefix = "https://maps.googleapis.com/maps/api/timezone/json?location=";

        NumberFormatInfo nfi = new NumberFormatInfo() { CurrencyDecimalSeparator = "." };

        public async Task<List<City>> GetCityList(string search)
        {
            return await Task.Factory.StartNew(() =>
            {
                List<City> list = null;
                try
                {
                    var searchUriString = citiesListRequestPrefix + search;
                    string str = null;

                    using (WebClient wc = new WebClient() { Encoding = Encoding.UTF8 })
                    {
                        str = wc.DownloadString(new Uri(searchUriString));
                    }

                    if (str != null)
                    {
                        var res = JObject.Parse(str);

                        list = res["response"]["docs"].Values<JObject>().Select(x => new City
                        {
                            name = (string)x["name_ascii"],
                            country = (string)x["country_name"],
                            timezone = ((string)x["timezone"]).Replace('_', ' '),
                            location = string.Format(nfi, "{0},{1}", x["lat"], x["lng"])
                        }).ToList();
                    }
                }
                catch (WebException ex)
                {
                    throw new Exception("Can't get data from API with search item [" + search + "].\nPlease check an internet connection.", ex) { Source = search };
                }
                catch (Exception x)
                {
                    throw new Exception("Something wrong while resolving[" + search + "].\n", x) { Source = search };
                }
                return list;
            });
        }

        public async Task<int> GetCityTimeOffset(City city)
        {
            return await Task.Factory.StartNew(() =>
            {
                int offset = 0;
                try
                {
                    var timezoneUri = timezoneRequesPrefix + city.location + "&timestamp=" + ConvertToUnixTimestamp(DateTime.UtcNow);

                    string timezoneString = null;

                    using (WebClient wc = new WebClient() { Encoding = Encoding.UTF8 })
                    {
                        timezoneString = wc.DownloadString(new Uri(timezoneUri));
                    }

                    if (timezoneString != null)
                        offset = (int)JObject.Parse(timezoneString)["rawOffset"] + (int)JObject.Parse(timezoneString)["dstOffset"];
                }
                catch (WebException ex)
                {
                    throw new Exception("Can't get data from API with search item [" + city + "].\nPlease check an internet connection.", ex) { Source = city.ToString()};
                }
                catch (Exception x)
                {
                    throw new Exception("Something wrong while resolving[" + city + "].\n", x) { Source = city.ToString() };
                }
                return offset;
            });
        }

        double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }

}
