using Authorization.EntityBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.BusinessLogic
{
    public interface IPermissionBL
    {
        public PermissionBE? GetPermission(int id);
        public bool UpdatePermission(PermissionBE permissionBe);
        public List<PermissionBE> ListPermission();
        public Task ProduceAsync(string topic, string message);
    }
}
