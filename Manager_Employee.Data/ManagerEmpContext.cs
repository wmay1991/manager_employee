using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Data.Entity;
using Manager_Employee.Data;
using Manager_Employee.Core;

namespace Manager_Employee.Data
{
    public class ManagerEmpContext : DbContext
    {
        public ManagerEmpContext()
            : base("name=Manager_Employees")
        {
        }

        public virtual DbSet<Manager> Manager { get; set; }

        public virtual DbSet<Employee> Employee { get; set; }
    }
}
