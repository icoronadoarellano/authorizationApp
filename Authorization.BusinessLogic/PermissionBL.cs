using Authorization.DataAccess;
using Authorization.EntityBusiness;
using System.Security;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;


namespace Authorization.BusinessLogic
{
    public class PermissionBL: IPermissionBL
    {
        private readonly IPermissionDA _permissionDa;
        private readonly IConfiguration _configuration;
        private readonly IProducer<Null, string> _producer;
        public PermissionBL(IPermissionDA permissionDa, IConfiguration configuration)
        {
            _permissionDa = permissionDa;
            _configuration = configuration;

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"]
            };
            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        }

        public async Task ProduceAsync(string topic, string message)
        {
            var kafkaMessage = new Message<Null, string> { Value = message, };
            await _producer.ProduceAsync(topic, kafkaMessage);
        }

        public PermissionBE? GetPermission(int id)
        {
            return _permissionDa.GetPermission(id);
        }

        public bool UpdatePermission(PermissionBE permissionBe)
        {
            permissionBe.EmployeeLastName = permissionBe.EmployeeLastName.ToUpper();
            permissionBe.EmployeeName = permissionBe.EmployeeName.ToUpper();
            return _permissionDa.UpdatePermission(permissionBe);
        }

        public List<PermissionBE> ListPermission()
        {
            return _permissionDa.ListPermission();
        }
    }
}
