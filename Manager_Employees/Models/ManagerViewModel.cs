using Manager_Employee.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Manager_Employees.Models
{
    public class ManagerViewModel
    {
        public ManagerViewModel()
        {

        }

        public ManagerViewModel(Manager model)
        {
            mgr_id = model.mgr_id;
            first_name = model.first_name;
            last_name = model.last_name;
        }


        public ManagerViewModel(Manager model, ManagerViewModel vm)
        {
            model.mgr_id = vm.mgr_id;
            model.first_name = vm.first_name;
            model.last_name = vm.last_name;
            model.employee = vm.employee;
        }


        public Guid mgr_id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [DisplayName("First Name")]
        public string first_name { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [DisplayName("Last Name")]
        public string last_name { get; set; }

        public IList<Employee> employee { get; set; }
    }
}