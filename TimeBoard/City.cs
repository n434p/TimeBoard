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
        public string offsetString { get
            {
                var sign = (offset >= 0) ? "+" : "-";
                var time = TimeSpan.FromSeconds(offset);
                return string.Format("UTC{0}{1:hh\\:mm}", sign, time);
            }
        }
        public int offset { get; set; }
        public List<string> Regions { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
