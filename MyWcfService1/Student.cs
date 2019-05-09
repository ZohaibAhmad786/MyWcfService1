using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWcfService1
{
    public class Student
    {
        public string email { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phoneNo { get; set; }
        public string address { get; set; }
        public string password { get; set; }
        public char gender { get; set; }
        public int semester { get; set; }
        public string discipline { get; set; }
        public string Error { get; set; }
        public string imgsrc { get; set; }
        public string CNIC { get; set; }
        public string Type { get; set; }
    }
}