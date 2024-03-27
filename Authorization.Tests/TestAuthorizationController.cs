using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Authorization.API.Controllers;
using Authorization.EntityBusiness;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Hosting;
using Nest;
using Microsoft.Extensions.Hosting.Internal;
using Authorization.BusinessLogic;
using Microsoft.OpenApi.Any;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;


namespace Authorization.Tests
{
    [TestClass]
    public  class TestAuthorizationController
    {
        private readonly Mock<IPermissionBL> _mockPermissionBl;
        private readonly Mock<IElasticClient> _mockElasticClient;
        private readonly Mock<IWebHostEnvironment> _mockHostinEnvironment;

        public TestAuthorizationController()
        {
            _mockPermissionBl = new Mock<IPermissionBL>();
            _mockElasticClient = new Mock<IElasticClient>();
            _mockHostinEnvironment = new Mock<IWebHostEnvironment>();
        }
        
        [TestMethod]
        public async Task GetPermission_ShouldReturnCorrectPermission()
        {
            var testPermissions = GetPermissions();
            _mockPermissionBl.Setup(e => e.GetPermission(1)).Returns(testPermissions.Find(e=>e.Id == 1));
            var controller = new AuthorizationController(_mockPermissionBl.Object, _mockElasticClient.Object, _mockHostinEnvironment.Object);
            var result = await controller.GetPermission(1);
            Assert.IsInstanceOfType<IActionResult>(result);
                            
        }
        [TestMethod]
        public async Task GetPermisions_ShouldReturnPermissionsList()
        {
            var testPermissions = GetPermissions();
            _mockPermissionBl.Setup(e => e.ListPermission())
                .Returns(testPermissions);
            var controller = new AuthorizationController(_mockPermissionBl.Object, _mockElasticClient.Object, _mockHostinEnvironment.Object);
            var result = await controller.ListPermission();
            Assert.IsInstanceOfType<IActionResult>(result);

        }
        [TestMethod]
        public async Task UpdatePermission_ShouldUpdatePermission()
        {
            var testPermissions = GetPermissions();
            _mockPermissionBl.Setup(e => e.UpdatePermission(testPermissions.First(e => e.Id == 1))).Returns(true);
            var controller = new AuthorizationController(_mockPermissionBl.Object, _mockElasticClient.Object, _mockHostinEnvironment.Object);
      
            var result = await controller.UpdatePermission(testPermissions.First(e => e.Id == 1));
            Assert.IsInstanceOfType<IActionResult>(result);

        }
        private List<PermissionBE> GetPermissions()
        {
            var testPermissionsBE = new List<PermissionBE>
            {
                new PermissionBE { Id = 1, EmployeeName     = "Isaac", EmployeeLastName = "Coronado", PermissionDate = DateTime.Now, PermissionTypeId = 1 },
                new PermissionBE { Id = 2, EmployeeName = "Alejandro", EmployeeLastName = "Coronado", PermissionDate = DateTime.Now, PermissionTypeId = 1 },
                new PermissionBE { Id = 3, EmployeeName = "Andres", EmployeeLastName = "Coronado", PermissionDate = DateTime.Now, PermissionTypeId = 1 }
            };

            return testPermissionsBE;
        }
    }
}
