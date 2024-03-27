using Authorization.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Arch.EntityFrameworkCore.UnitOfWork;
using Authorization.DataAccess.Models;
using Authorization.EntityBusiness;

namespace Authorization.DataAccess
{
    public class PermissionDA: IPermissionDA
    {
        private readonly IUnitOfWork<PermissionContext> _unitOfWork;

        public PermissionDA(IUnitOfWork<PermissionContext> unitOfWork) { _unitOfWork = unitOfWork; }

        public PermissionBE? GetPermission(int id)
        {
            PermissionBE? permission = null;

            var result = _unitOfWork.DbContext.Permissions.Where(a => a.Id == id).FirstOrDefault();

            if(result != null)
            {
                permission = new PermissionBE
                {
                    EmployeeLastName = result.EmployeeLastName,
                    EmployeeName = result.EmployeeName,
                    Id = result.Id,
                    PermissionDate = result.PermissionDate,
                    PermissionTypeId = result.PermissionTypeId
                };
            }

            return permission;
        }

        public bool UpdatePermission(PermissionBE permissionBe) {
            var permission = new Permission
            {
                Id = permissionBe.Id,
                EmployeeLastName = permissionBe.EmployeeLastName,
                EmployeeName = permissionBe.EmployeeName,
                PermissionDate = permissionBe.PermissionDate,
                PermissionTypeId = permissionBe.PermissionTypeId
            };

            var countRows = 0;

            try
            {
                var result = _unitOfWork.DbContext.Permissions.Update(permission);

                countRows = _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }

            return countRows > 0;
        }

        public List<PermissionBE> ListPermission()
        {
            List<PermissionBE> list = new List<PermissionBE>();

            var result = _unitOfWork.DbContext.Permissions.ToList();

            if(result.Count > 0)
            {
                result.ForEach(p => list.Add(new PermissionBE
                {
                    EmployeeLastName = p.EmployeeLastName,
                    EmployeeName = p.EmployeeName,
                    PermissionDate = p.PermissionDate,
                    PermissionTypeId = p.PermissionTypeId,
                    Id = p.Id,
                }));
            }

            return list;
        }

    }
}
