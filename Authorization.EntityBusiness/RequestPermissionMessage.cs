using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.EntityBusiness
{
    public class RequestPermissionMessage
    {
        public Guid Id { get; set; }
        public string OperationName { get; set; }
        public PermissionBE ?Permission { get; set; }
        public List<PermissionBE> ?Permissions { get; set; }
    }
}
