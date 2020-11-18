using BrokerR.Http.Client.Demo.Hosting.CRM;
using JetBrains.Annotations;

namespace BrokerR.Http.Client.Demo
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature, ImplicitUseTargetFlags.WithMembers)]
    public sealed class CRMOptions : ICRMOptions
    {
        public const string CRM = "CRM";
        
        public string Environment { get; set; } = "uat";
    }
}