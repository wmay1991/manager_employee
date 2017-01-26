using System;
using NUnit.Compatibility;
using NUnit.Framework;
using Manager_Employee.Core;
using Moq;
using System.Data.Entity;
using Manager_Employee.Data;
using Manager_Employees.Controllers;
using Manager_Employees.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace Manager_Employees.Tests.Controllers
{
    [TestFixture]
    public class EmployeeTest
    {

        [Test]
        public void ViewAddEmployee()
        {
            var _dbManager = new ManagerEmpContext();
            var mgr = _dbManager.Manager.Where(m => m.last_name == "Doe").SingleOrDefault();
            var controller = new EmployeeController();
            var result = controller.AddEmployee(mgr.mgr_id);

            Assert.IsAssignableFrom(typeof(ViewResult), result);
        }

        [Test]
        public void ViewAddEmployeeInvalid()
        {
            var controller = new EmployeeController();
            var result = controller.AddEmployee(new Guid());

            //make sure it throws an exception
            Assert.IsAssignableFrom(typeof(HttpNotFoundResult), result);
        }

        [Test]
        public void AddEmployee()
        {
            var existing_mgr = new Manager
            {
                first_name = "John",
                last_name = "Baker",
                mgr_id = Guid.NewGuid()
            };

            var emp_id = Guid.NewGuid();
            var emp = new Employee
            {
                emp_id = emp_id,
                first_name = "Sam",
                last_name = "Frank",
                mgr_id = existing_mgr.mgr_id,
                mgr = existing_mgr,
                dept = "IT",
                start_date = DateTime.Now,
                title = "Network Engineer"
            };

            var vm = new EmployeeViewModel(emp);
            var mockEmp = new Mock<DbSet<Employee>>();
            mockEmp.Setup(m => m.Find(emp_id)).Returns(emp);

            var mockEmpContext = new Mock<ManagerEmpContext>();
            mockEmpContext.Setup(m => m.Employee).Returns(mockEmp.Object);

            var controller1 = new EmployeeController(mockEmpContext.Object);
            controller1.AddEmployee(vm);

            mockEmp.Verify(m => m.Add(It.IsAny<Employee>()), Times.Once());
        }


        [Test]
        public void EditEmployee()
        {
            var existing_mgr = new Manager
            {
                first_name = "Jessica",
                last_name = "Whitt",
                mgr_id = Guid.NewGuid()
            };

            var emp_id = Guid.NewGuid();
            var existing_emp = new Employee
            {
                emp_id = emp_id,
                first_name = "Sam",
                last_name = "Frank",
                mgr_id = existing_mgr.mgr_id,
                mgr = existing_mgr,
                dept = "Accounting",
                start_date = DateTime.Now,
                title = null
            };

            var updated_emp = new Employee
            {
                emp_id = emp_id,
                first_name = "Sam",
                last_name = "Franklin",
                mgr_id = existing_mgr.mgr_id,
                mgr = existing_mgr,
                dept = "Marketing",
                start_date = DateTime.Now
            };

            var vm = new EmployeeViewModel(updated_emp);

            // update item
            var mockItem = new Mock<DbSet<Employee>>();
            mockItem.Setup(x => x.Find(emp_id)).Returns(existing_emp);


            var mockContext = new Mock<ManagerEmpContext>();
            mockContext.Setup(x => x.Employee).Returns(mockItem.Object);

            var controller = new EmployeeController(mockContext.Object);
            controller.EditEmployee(vm);

            Assert.That(mockContext.Object.Employee.Find(emp_id).last_name == "Franklin");
            Assert.That(mockContext.Object.Employee.Find(emp_id).dept == "Marketing");
        }

        [Test]
        public void ViewEditEmployee()
        {
            // had to add a few managers for dropdown list on edit
            var mgr_id = Guid.NewGuid();
            var mgr = new List<Manager>
            {
                 new Manager { mgr_id = mgr_id,  first_name = "Test1", last_name = "LastName1"},
                new Manager {mgr_id = Guid.NewGuid(), first_name = "Test2", last_name = "LastName2" },
                new Manager { mgr_id = Guid.NewGuid(), first_name = "Test3", last_name = "LastName2"},
            }.AsQueryable();

            var mockMgr1 = new Mock<DbSet<Manager>>();
            mockMgr1.As<IQueryable<Manager>>().Setup(m => m.Provider).Returns(mgr.Provider);
            mockMgr1.As<IQueryable<Manager>>().Setup(m => m.Expression).Returns(mgr.Expression);
            mockMgr1.As<IQueryable<Manager>>().Setup(m => m.ElementType).Returns(mgr.ElementType);
            mockMgr1.As<IQueryable<Manager>>().Setup(m => m.GetEnumerator()).Returns(mgr.GetEnumerator());

            var id = Guid.NewGuid();
            var emp = new Employee
            {
                emp_id = id,
                first_name = "Wendy",
                last_name = "Doe",
                mgr_id = mgr.ElementAt(0).mgr_id,
                mgr = mgr.ElementAt(0),
                dept = "Marketing",
                start_date = DateTime.Now,
                title = "Inside Sales Rep"
            };

            var mockEmp = new Mock<DbSet<Employee>>();
            mockEmp.Setup(x => x.Find(id)).Returns(emp);


            var mockMgr = new Mock<DbSet<Manager>>();
            mockMgr.Setup(x => x.Find(mgr_id)).Returns(mgr.ElementAt(0));


            var mockEmpContext = new Mock<ManagerEmpContext>();
            mockEmpContext.Setup(x => x.Employee).Returns(mockEmp.Object);
            mockEmpContext.Setup(x => x.Manager).Returns(mockMgr1.Object);


            var controller = new EmployeeController(mockEmpContext.Object);
            var result = controller.EditEmployee(id) as ViewResult;
            var model = result.ViewData.Model as EmployeeViewModel;
            //make sure the view returns something
            Assert.IsTrue(mockEmpContext.Object.Employee.Find(id).emp_id == id);
            Assert.IsAssignableFrom(typeof(ViewResult), result);
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsNotNull(result.ViewData.Model);
            Assert.AreEqual(mockEmpContext.Object.Manager.Count(), 3);

        }

        [Test]
        public void ViewEditEmployeeInvalid()
        {
            var controller = new EmployeeController();
            controller.AddEmployee(new Guid());

            Assert.IsAssignableFrom(typeof(HttpNotFoundResult), controller.AddEmployee(new Guid()));
        }


        [Test]
        public void ShowEmployeeList()
        {
            var id = Guid.NewGuid();
            var emp = new List<Employee>
            {
                 new Employee { emp_id = Guid.NewGuid(), mgr_id = id, first_name = "Test1", last_name = "LastName1", dept = "IT", start_date = DateTime.Now , title = null},
                new Employee {emp_id = Guid.NewGuid(),mgr_id = id ,first_name = "Test2", last_name = "LastName2", dept = "IT", start_date = DateTime.Now, title = "DBA"},
                new Employee { emp_id = Guid.NewGuid(), mgr_id = id, first_name = "Test3", last_name = "LastName2", dept = "IT", start_date = DateTime.Now,  title = "Service Desk Analyst"},
            }.AsQueryable();

            var mgr = new Manager
            {
                mgr_id = id,
                first_name = "Bob",
                last_name = "Turner",
                employee = emp.ToList()
            };


            var mockEmp = new Mock<DbSet<Employee>>();
            mockEmp.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(emp.Provider);
            mockEmp.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(emp.Expression);
            mockEmp.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(emp.ElementType);
            mockEmp.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(emp.GetEnumerator());

            var mockEmpContext = new Mock<ManagerEmpContext>();
            mockEmpContext.Setup(e => e.Employee).Returns(mockEmp.Object);

            var controller = new EmployeeController();
            controller.Index();

            Assert.AreEqual(mockEmpContext.Object.Employee.Count(), 3);
        }

        [Test]
        public void ViewEmployeeDetails()
        {
            var existing_mgr = new Manager
            {
                first_name = "Todd",
                last_name = "Thomas",
                mgr_id = Guid.NewGuid()
            };

            var id = Guid.NewGuid();
            var emp = new Employee
            {
                emp_id = id,
                first_name = "Rachel",
                last_name = "Cooper",
                mgr_id = existing_mgr.mgr_id,
                mgr = existing_mgr,
                dept = "IT",
                start_date = DateTime.Now,
                title = "Web Designer"

            };


            var mockEmp = new Mock<DbSet<Employee>>();
            mockEmp.Setup(x => x.Find(id)).Returns(emp);


            var mockEmpContext = new Mock<ManagerEmpContext>();
            mockEmpContext.Setup(x => x.Employee).Returns(mockEmp.Object);


            var controller = new EmployeeController(mockEmpContext.Object);
            var result = controller.Details(id);


            Assert.IsNotNull(result);
            Assert.IsTrue(mockEmpContext.Object.Employee.Find(id).first_name == "Rachel");
        }

        [Test]
        public void ViewEmployeeDetailsInvalid()
        {

            var controller = new EmployeeController();
            var result = controller.Details(new Guid());

            //make sure it throws an exception
            Assert.IsAssignableFrom(typeof(HttpNotFoundResult), result);
        }

    }
}
