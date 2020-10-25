using System;

namespace BrokerR.Http.Client.Demo
{
    public class AccountChanged
    {
        public Guid Id { get; set; }

        public static AccountChanged Create(Guid id)
        {
            return new AccountChanged
            {
                Id = id
            };
        }
    }
}