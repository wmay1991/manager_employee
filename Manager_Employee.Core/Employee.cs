using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager_Employee.Core
{
    public class Employee
    {

        [Key]
        public Guid emp_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    
        public string dept { get; set; }

        public string title { get; set; }

        public DateTime? start_date { get; set; }
        public Guid  mgr_id { get; set; }

        public virtual Manager mgr { get; set; }

    }
}
