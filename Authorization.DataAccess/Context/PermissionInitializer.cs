using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authorization.DataAccess.Models;
using Microsoft.EntityFrameworkCore;


namespace Authorization.DataAccess.Context
{
    public class PermissionInitializer
    {
        private readonly PermissionContext _context;

        public PermissionInitializer(PermissionContext context)
        {
            _context = context;
        }

        public void Run()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            var permissionTypes = new List<PermissionType>
            {
                new PermissionType{Description="Admin"},
                new PermissionType{Description="Guest"},
                new PermissionType{Description="User"}
            };

            permissionTypes.ForEach(permissionType =>  _context.PermissionTypes.Add(permissionType) );
            
            _context.SaveChanges();

            var permissions = new List<Permission>
            {
                new Permission{EmployeeName="Isaac", EmployeeLastName="Coronado", PermissionDate=DateTime.Now, PermissionTypeId= 1}
            };

            permissions.ForEach(permission  => _context.Permissions.Add(permission) );
            
            _context.SaveChanges();

        }
    }
}
