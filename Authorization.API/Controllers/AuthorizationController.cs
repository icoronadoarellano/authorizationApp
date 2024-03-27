// using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Authorization.DataAccess;
using Authorization.EntityBusiness;
using Authorization.BusinessLogic;
using Microsoft.AspNetCore.Cors;
using System.Text.Json;
using Authorization.DataAccess.Models;
using Nest;
using System.Security;

namespace Authorization.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IPermissionBL _permissionBl;
        private readonly IElasticClient _elasticClient;
        private readonly IWebHostEnvironment _hostingEnvironment;
        
        public AuthorizationController(IPermissionBL permissionBl, IElasticClient elasticClient, IWebHostEnvironment hostingEnvironment)
        {
            _permissionBl = permissionBl;
            _elasticClient = elasticClient;
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpGet]

        [Route("{id}")]
        public async Task<IActionResult> GetPermission(int id)
        {
            try
            {
                var permission = _permissionBl.GetPermission(id);
                RequestPermissionMessage requestPermission = new RequestPermissionMessage();
                requestPermission.Id = Guid.NewGuid();
                requestPermission.OperationName = "REQUEST";
                requestPermission.Permission = permission;
                
                string message = JsonSerializer.Serialize(requestPermission);
                await _permissionBl.ProduceAsync("Permission", message);
                await _elasticClient.IndexDocumentAsync(permission);
                return permission != null ? Ok(permission): NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        public async Task<IActionResult> UpdatePermission([FromBody] PermissionBE permission)
        {
            try
            {
                _permissionBl.UpdatePermission(permission);
                RequestPermissionMessage requestPermission = new RequestPermissionMessage();
                requestPermission.Id = Guid.NewGuid();
                requestPermission.OperationName = "MODIFY";
                requestPermission.Permission = permission;

                string message = JsonSerializer.Serialize(requestPermission);
                await _permissionBl.ProduceAsync("Permission", message);
                await _elasticClient.IndexDocumentAsync(permission);
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListPermission()
        {
            try
            {
                var permissions = _permissionBl.ListPermission();
                RequestPermissionMessage requestPermission = new RequestPermissionMessage();
                requestPermission.Id = Guid.NewGuid();
                requestPermission.OperationName = "GET";
                requestPermission.Permissions = permissions;

                string message = JsonSerializer.Serialize(requestPermission);
                await _permissionBl.ProduceAsync("Permission", message);
                if (permissions !=null && permissions.Count > 0)
                {
                    foreach(var permission in permissions)
                    {
                        await _elasticClient.IndexDocumentAsync(permission);

                    }
                }
                return Ok(permissions);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
