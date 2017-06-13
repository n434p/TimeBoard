using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace TimeBoard.TimeProviders
{
    public interface ITimeProvider
    {
        void GetCityList(string searchString, Action<List<City>> callback);
        void GetCityInfo(string searchString, Action<City> callback);
    }
}
