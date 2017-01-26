using System;
using NUnit.Framework;
using NUnit.Compatibility;
using Manager_Employee.Core;
using Moq;
using Manager_Employee.Data;
using Manager_Employees.Controllers;
using System.Collections.Generic;
using Manager_Employees.Models;
using System.Data.Entity;
using System.Web.Mvc;
using System.Linq;

namespace Manager_Employees.Tests.Controllers
{
    [TestFixture]
    public class ManagerTest
    {
        /*ADD */
         [Test]
         public void ViewAddManager()
        {
            var controller = new ManagerController();
            controller.AddManager();

            Assert.IsAssignableFrom(typeof(ViewResult), controller.AddManager());

        }

        [Test]
        public void AddManager()
        {
            var mgr = new Manager
            {
                mgr_id = Guid.NewGuid(),
                first_name = "Julie",
                last_name = "May"
            };

            var vm = new ManagerViewModel(mgr);
            var mockItem = new Mock<DbSet<Manager>>();

            var mockContext = new Mock<ManagerEmpContext>();
            mockContext.Setup(x => x.Manager).Returns(mockItem.Object);

            var controller = new ManagerController(mockContext.Object);
            var result = controller.AddManager(vm) as RedirectToRouteResult;

            // test to make sure it is added and goes to the correct view
            mockItem.Verify(m => m.Add(It.IsAny<Manager>()), Times.Once());
            Assert.AreEqual(result.RouteValues["action"], "Index");
        }

        /*EDIT */
        [Test]
        public void EditManager()
        {
            var id = Guid.NewGuid();
            var existing_mgr = new Manager
            {
                mgr_id = id,
                first_name = "Julie",
                last_name = "May"
            };

            var updated_mgr = new Manager
            {
                mgr_id = id,
                first_name = "Julie",
                last_name = "Maynard"
            };


            var vm = new ManagerViewModel(updated_mgr);

            var mockItem = new Mock<DbSet<Manager>>();
            mockItem.Setup(x => x.Find(id)).Returns(existing_mgr);

            var mockContext = new Mock<ManagerEmpContext>();
            mockContext.Setup(x => x.Manager).Returns(mockItem.Object);

            var controller = new ManagerController(mockContext.Object);
            controller.EditManager(vm);

            mockContext.Verify(x => x.SaveChanges());
            Assert.That(mockContext.Object.Manager.Find(id).last_name == "Maynard");
        }

        [Test]
        public void ViewEditManager()
        {
            var id = new Guid();
            var existing_mgr = new Manager
            {
                mgr_id = new Guid(),
                first_name = "Wendy",
                last_name = "Doe"
            };

            var mockManager = new Mock<DbSet<Manager>>();
            mockManager.Setup(x => x.Find(id)).Returns(existing_mgr);

            var mockManagerContext = new Mock<ManagerEmpContext>();
            mockManagerContext.Setup(x => x.Manager).Returns(mockManager.Object);

            var controller = new ManagerController(mockManagerContext.Object);
            var result = controller.EditManager(id) as ViewResult;

            //make sure the view returns something
            Assert.IsTrue(mockManagerContext.Object.Manager.Find(id).mgr_id == id);
            Assert.IsAssignableFrom(typeof(ViewResult), result);

        }

        [Test]
        public void ViewEditManagerInvalid()
        {
       
            var controller = new ManagerController();
            var result = controller.EditManager(new Guid());

            //make sure it throws an exception
            Assert.IsAssignableFrom(typeof(HttpNotFoundResult), result);

        }
 
        /*DETAILS*/
        [Test]
        public void ViewManagerDetails() {
            var mgr_db = new ManagerEmpContext();
            var mgr = mgr_db.Manager.Where(m => m.last_name == "Doe").First();
            var controller = new ManagerController();
           var result =  controller.Details(mgr.mgr_id);

            Assert.IsNotNull(result);
            Assert.That(mgr.first_name == "Janet");

        }

        [Test]
        public void ViewManagerDetailsInvalid()
        {;
            var controller = new ManagerController();
            var result = controller.Details(new Guid());

            //make sure it throws an exception
            Assert.IsAssignableFrom(typeof(HttpNotFoundResult), result);

        }

        /*REMOVE EMP */
        [Test]
        public void RemoveEmployee()
        {
            var existing_mgr = new Manager {
                first_name = "John",
                last_name = "Baker",
                mgr_id = Guid.NewGuid()
            };

            var id = Guid.NewGuid();
            var emp = new Employee
            {
                emp_id = id,
                first_name = "Sally",
                last_name = "Smith",
                mgr_id = existing_mgr.mgr_id,
                mgr = existing_mgr
            };

            var vm = new EmployeeViewModel(emp);

            var mockItem = new Mock<DbSet<Employee>>();
            mockItem.Setup(x => x.Find(id)).Returns(emp);


            var mockContext = new Mock<ManagerEmpContext>();
            mockContext.Setup(x => x.Employee).Returns(mockItem.Object);

            var controller = new ManagerController(mockContext.Object);
            controller.RemoveEmployee(id);

            mockContext.Verify(x => x.SaveChanges());
        }


        [Test]
        public void RemoveEmployeeInvalid()
        {
            var controller = new ManagerController();
            controller.RemoveEmployee(new Guid());

            //make sure it throws an exception
            Assert.IsAssignableFrom(typeof(HttpNotFoundResult), controller.RemoveEmployee(new Guid()));
        }

        /*READ */
        [Test]
        public void ViewManagers()
        {
            var mdl = new List<Manager>
            {
                 new Manager { mgr_id = Guid.NewGuid() , first_name = "Test1", last_name = "LastName1"},
                new Manager {mgr_id = Guid.NewGuid() , first_name = "Test2", last_name = "LastName2"},
                new Manager { mgr_id = Guid.NewGuid(), first_name = "Test3", last_name = "LastName2"},
            }.AsQueryable();


            var mockMgr = new Mock<DbSet<Manager>>();
            mockMgr.As<IQueryable<Manager>>().Setup(m => m.Provider).Returns(mdl.Provider);
            mockMgr.As<IQueryable<Manager>>().Setup(m => m.Expression).Returns(mdl.Expression);
            mockMgr.As<IQueryable<Manager>>().Setup(m => m.ElementType).Returns(mdl.ElementType);
            mockMgr.As<IQueryable<Manager>>().Setup(m => m.GetEnumerator()).Returns(mdl.GetEnumerator());


            var mockMgrContext = new Mock<ManagerEmpContext>();
            mockMgrContext.Setup(m => m.Manager).Returns(mockMgr.Object);

            ManagerController controller = new ManagerController(mockMgrContext.Object);
            controller.Index();
            Assert.AreEqual(mockMgrContext.Object.Manager.Where(m => m.first_name == "Test1").Count(), 1);
            Assert.AreEqual(mockMgrContext.Object.Manager.Where(m => m.last_name == "LastName2").Count(), 2);
            Assert.AreEqual(mockMgrContext.Object.Manager.Count(), 3);
        }

        /*DELETE*/
        [Test]
        public void DeleteManager()
        {
            var id = Guid.NewGuid();
            var del_mgr = new Manager()
            {
                mgr_id = id,
                first_name = "Sam",
                last_name = "Johnson"
            };

            var vm = new ManagerViewModel(del_mgr);

            var mockItem = new Mock<DbSet<Manager>>();
            mockItem.Setup(x => x.Find(id)).Returns(del_mgr);

            var mockContext = new Mock<ManagerEmpContext>();
            mockContext.Setup(x => x.Manager).Returns(mockItem.Object);

            var controller = new ManagerController(mockContext.Object);
            controller.DeleteManager(id);

            mockContext.Verify(x => x.SaveChanges());
        }


        [Test]
        public void DeleteManagerValid()
        {

            var controller = new ManagerController();
            controller.DeleteManager(new Guid());

            //make sure it throws an exception
            Assert.IsAssignableFrom(typeof(HttpNotFoundResult), controller.DeleteManager(new Guid()));
        }
    }
}