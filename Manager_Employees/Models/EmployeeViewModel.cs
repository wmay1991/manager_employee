using Manager_Employee.Core;
using Manager_Employee.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manager_Employees.Models
{
    public class EmployeeViewModel
    {
        public EmployeeViewModel()
        {

        }

        public EmployeeViewModel(Employee model)
        {
            emp_id = model.emp_id;
            first_name = model.first_name;
            last_name = model.last_name;
            mgr_id = model.mgr_id;
            mgr = model.mgr;
            dept = model.dept;
            start_date = model.start_date;
            title = model.title;
        }


        public EmployeeViewModel(Employee model, EmployeeViewModel vm)
        {
            model.emp_id = vm.emp_id;
            model.first_name = vm.first_name;
            model.last_name = vm.last_name;
            model.mgr_id = vm.mgr_id;
            model.mgr = vm.mgr;
            model.dept = vm.dept;
            model.start_date = vm.start_date;
            model.title = vm.title;
        }
        public Guid emp_id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [DisplayName("First Name")]
        public string first_name { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [DisplayName("Last Name")]
        public string last_name { get; set; }

        public Guid mgr_id { get; set; }

        public Manager mgr { get; set; }

        [Required(ErrorMessage = "Please select a department")]
        [DisplayName("Department")]
        public string dept { get; set; }

        [DisplayName("Title")]
        public string title { get; set; }

        [Required(ErrorMessage = "Please enter a start date")]
        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        public DateTime? start_date { get; set; }

        // added to viewmodel to enable resuse and scability (easy to add an addtional value)
        // could possibly replace with the db table in the future but leaving static values for now
        public IEnumerable<SelectListItem> Departments
        {
            get
            {return new[]
                {
                new SelectListItem { Value = "IT", Text = "IT" },
                new SelectListItem { Value = "Accounting", Text = "Accounting" },
                new SelectListItem { Value = "Marketing", Text = "Marketing" },
                };
            }
        }
    }
  }