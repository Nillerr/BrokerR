using System;
using JetBrains.Annotations;

namespace BrokerR.Http.Client.Demo
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature, ImplicitUseTargetFlags.WithMembers)]
    public sealed class CRMWebhookRequest
    {
        public Guid PrimaryEntityId { get; set; }
    }
}