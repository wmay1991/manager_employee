using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager_Employee.Core
{
    public class Manager
    {

        public Manager ()
        {
        }

        [Key]
        public Guid mgr_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }

        // Would like to add, modify, and sort
        public IList<Employee> employee { get; set; }

    }
}
