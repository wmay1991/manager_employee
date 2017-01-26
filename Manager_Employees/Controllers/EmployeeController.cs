using Manager_Employee.Core;
using Manager_Employee.Data;
using Manager_Employees.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Manager_Employees.Controllers
{
    public class EmployeeController : Controller
    {

        private ManagerEmpContext _db = new ManagerEmpContext();

        public EmployeeController(ManagerEmpContext db)
        {
            _db = db;
        }


        public EmployeeController()
        {
            _db = new ManagerEmpContext();
        }
        // GET: Employee
        public ActionResult Index()
        {
            // return all employees
            var emp_list = _db.Employee.ToList().OrderBy(e => e.last_name);

            return View(emp_list);
        }

        public ActionResult AddEmployee(Guid mgr_id)
        {
            // just in case someone tries to type url manually without a valid id
            var mgr = _db.Manager.Find(mgr_id);

            if (mgr == null)
            {
                return HttpNotFound();
            }
            
            // access this through manager page
            var emp = new Employee
            {
                emp_id = Guid.NewGuid(),
                first_name = null,
                last_name = null,
                mgr_id = mgr.mgr_id,
                mgr = mgr
            };

            var vm = new EmployeeViewModel(emp);
            
            return View(vm);
        }

        [HttpPost]
        public ActionResult AddEmployee(EmployeeViewModel vm)
        {
            if (ModelState.IsValid) {
                var emp = new Employee();
                //needs to populate the model
                var mdl = new EmployeeViewModel(emp, vm);
                _db.Employee.Add(emp);
                _db.SaveChanges();

                // Send back to the employees manager page
                return RedirectToAction("Index", "Manager", new { mgr_id = vm.mgr_id });
            }
            else
            {
                return View();
            }
        }

        // GET: Employee/Details/5
        public ActionResult Details(Guid emp_id)
        {
            var emp = _db.Employee.Find(emp_id);
            // just in case someone tries to type url manually without a valid id
            if (emp == null)
            {
                return HttpNotFound();
            }

            var vm = new EmployeeViewModel(emp);
            return View(vm);
        }


        // GET: Employee/Edit/5
        public ActionResult EditEmployee(Guid emp_id)
        {
            // just in case someone tries to type url manually without a valid id
            var emp = _db.Employee.Find(emp_id);
            GetManagers();


            if (emp == null)
            {
                return HttpNotFound();
            }

            var vm = new EmployeeViewModel(emp);

            return View(vm);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult EditEmployee(EmployeeViewModel vm)
        {
            try
            {
                var mdl_emp = _db.Employee.Find(vm.emp_id);
                var edit_emp = new EmployeeViewModel(mdl_emp, vm);


               _db.Entry(mdl_emp).State = EntityState.Modified;
                _db.SaveChanges();

                return RedirectToAction("Index", "Manager", new { mgr_id = vm.mgr_id });
            }
            catch
            {
                return View();
            }
        }

        
        public void GetManagers()
        {
            var mgr_list = _db.Manager.ToList().OrderBy(m => m.last_name);

            // create a manager dropdown and add to view bag
            // Value has to be a string so had to convert 
            IEnumerable<SelectListItem> list = from m in mgr_list select new SelectListItem { Value = m.mgr_id.ToString(), Text = m.first_name + " " + m.last_name };
            ViewBag.mgr_list_id = new SelectList(list, "Value", "Text");
        }
    }
}
