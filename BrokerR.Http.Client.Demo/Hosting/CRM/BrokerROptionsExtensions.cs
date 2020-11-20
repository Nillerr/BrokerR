using System;
using BrokerR.Http.Client.Hosting;

namespace BrokerR.Http.Client.Demo.Hosting.CRM
{
    public static class BrokerROptionsExtensions
    {
        public static BrokerROptions WithCRM(
            this BrokerROptions options,
            ICRMOptions configuration,
            Action<BrokerRCRMOptions> configure
        )
        {
            var crm = new BrokerRCRMOptions(options, configuration);
            configure(crm);
            return options;
        }
    }
}