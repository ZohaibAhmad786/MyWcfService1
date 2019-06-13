using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace MyWcfService1
{
    public class Topics
    {
        public string semail { get; set; }
        public List<Topics> subtopics { get; set; }

        public string maintopics { get; set; }
        
        public int topicstatus { get; set; }
        public string datetimetoday { get; set; }
        public string subject { get; set; }
        public string day { get; set; }
        public string heldStatus { get; set; }
        public string timming { get; set; }
        public int id { get; set; }
        public string status { get; set; }

        public string TuEmail { get; set; }

    }
    
}