using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeBoard
{
    public class City
    {
        public string id { get; set; }
        public string name { get; set; }
        public string offsetString { get; set; }
        public int offset { get; set; }
        public List<string> Regions { get; set; }

        public override string ToString()
        {
            return name;
        }

        public string RegionsLines()
        {
            var str = "";

            foreach (var item in Regions)
            {
                str += item+"\n";
            }
            return str;
        }
    }
}
