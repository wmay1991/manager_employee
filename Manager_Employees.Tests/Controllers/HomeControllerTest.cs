using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Manager_Employees;
using Manager_Employees.Controllers;
using NUnit.Framework;
using Moq;
using NUnit.Compatibility;
using Manager_Employee.Core;
using System.Data.Entity;
using Manager_Employee.Data;
using Manager_Employees.Models;

namespace Manager_Employees.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void ReturnView()
        {
            // make sure something is returned
            var controller = new HomeController();
            var result = controller.Index();
            Assert.IsNotNull(result);
        }
      
    }
}
