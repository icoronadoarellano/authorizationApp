using Authorization.EntityBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.DataAccess
{
    public interface IPermissionDA
    {

        public PermissionBE? GetPermission(int id);
        public bool UpdatePermission(PermissionBE permissionBe);
        public List<PermissionBE> ListPermission();
    }
}
