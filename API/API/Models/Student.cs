using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Student
    {
        public string stundentID { get; set; }

        public string name { get; set; }
        
        public string[] enrolledClasses { get; set; }
    }
}
