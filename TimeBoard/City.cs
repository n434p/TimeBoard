using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TimeBoard
{
    [DataContract]
    public class City
    {
        [DataMember]
        public string location { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string timezone { get; set; }
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public int offset { get; set; }
        public string offsetString { get
            {
                var sign = (offset >= 0) ? "+" : "-";
                var time = TimeSpan.FromSeconds(offset);
                return string.Format("UTC{0}{1:hh\\:mm}", sign, time);
            }
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}",name, country,timezone);
        }
    }
}
