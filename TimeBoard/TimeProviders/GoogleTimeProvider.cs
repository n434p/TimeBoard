using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeBoard.TimeProviders
{
    class GoogleTimeProvider : ITimeProvider
    {
        public void GetCityInfo(string searchString, Action<City> callback)
        {
            throw new NotImplementedException();
        }

        public void GetCityList(string searchString, Action<List<City>> callback)
        {
            throw new NotImplementedException();
        }
    }
}
