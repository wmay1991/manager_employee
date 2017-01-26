using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manager_Employee.Core;
using Manager_Employees.Models;
using Manager_Employee.Data;
using System.Data.Entity;

namespace Manager_Employees.Controllers
{
    public class ManagerController : Controller
    {
        private ManagerEmpContext _db = new ManagerEmpContext();
        public ManagerController(ManagerEmpContext db)
        {
            _db = db;
        }


        public ManagerController()
        {
            _db = new ManagerEmpContext();
        }

        public ActionResult Index()
        {
            var mgrs_list = _db.Manager.ToList();

            return View(mgrs_list);
        }

        public ActionResult DeleteManager(Guid itemId)
        {
            var deleting_manager = _db.Manager.Find(itemId);

            if (deleting_manager == null)
            {
                return HttpNotFound();
            }

            _db.Manager.Remove(deleting_manager);
            _db.Entry(deleting_manager).State = EntityState.Deleted;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Manager
        public ActionResult Details(Guid mgr_id)
        {
            var model = _db.Manager.Find(mgr_id);
            if (model == null)
            {
                return HttpNotFound();
            }

            var emp_list = _db.Employee.Where(m => m.mgr_id == model.mgr_id).OrderBy(m => m.last_name).ToList();

            var viewModel = new ManagerViewModel
            {
                employee = emp_list,
                first_name = model.first_name,
                last_name = model.last_name,
                mgr_id = model.mgr_id
            };

 
            return View(viewModel);
        }

        // GET: Manager/Create
        public ActionResult AddManager()
        {
            return View();
        }

        // POST: Manager/Create
        [HttpPost]
        public ActionResult AddManager(ManagerViewModel vm)
        {
            // make sure all of the fields are populated
            if (ModelState.IsValid)
            { 
                var mgr = new Manager();
                vm.mgr_id = Guid.NewGuid();
                var model = new ManagerViewModel(mgr, vm);

                _db.Manager.Add(mgr);
                _db.SaveChanges();

                return RedirectToAction("Index","Home");
            }
            else
            {
                return View();
            }
        }

        // GET: Manager/Edit/5
        public ActionResult EditManager(Guid itemId)
        {
            var mgr = _db.Manager.Find(itemId);
            if (mgr == null)
            {
                return HttpNotFound();
            }

            var vm = new ManagerViewModel(mgr);
            return View(vm);
        }

        // POST: Manager/Edit/5
        [HttpPost]
        public ActionResult EditManager(ManagerViewModel vm)
        {
            if (ModelState.IsValid)
            { 
            var mgr_to_update = _db.Manager.Find(vm.mgr_id);
            var updated_mgr = new ManagerViewModel(mgr_to_update, vm);

            _db.Entry(mgr_to_update).State = EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
            else
        {
            return View();
        }
    }


        public ActionResult RemoveEmployee(Guid emp_id)
        {
            var emp = _db.Employee.Find(emp_id);

            // just in case someone tries to type url manually without a valid id
            if (emp == null)
            {
                return HttpNotFound();
            }

            _db.Employee.Remove(emp);
            _db.Entry(emp).State = EntityState.Deleted;
            _db.SaveChanges();
            return RedirectToAction("Index",new { mgr_id = emp.mgr_id });
        }

    }
}
