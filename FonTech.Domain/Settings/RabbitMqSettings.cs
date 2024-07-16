using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Domain.Settings
{
    public class RabbitMqSettings
    {
        public string QueueName { get; set; }

        public string RoutingName { get; set; }

        public string ExchangeName { get; set; }
    }
}
