using System;
using System.Runtime.Serialization;

namespace Domain
{
    [DataContract(IsReference = true)]
    public class User
    {
        [DataMember]
        public Guid UserId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        //password stored as string for example
        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public DateTime DateCaptured { get; set; }
    }
}