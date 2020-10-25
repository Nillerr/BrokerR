using System;

namespace BrokerR.Http.Client.Demo
{
    public class ContactChanged
    {
        public Guid Id { get; set; }

        public static ContactChanged Create(Guid id)
        {
            return new ContactChanged
            {
                Id = id
            };
        }
    }
}