using System;
using System.Runtime.Serialization;

namespace PetClubLib.Models
{
    [DataContract]
    public abstract class ModelBase : INotification
    {
        [DataMember]
        public DateTime Timestamp { get; set; }
    }

    public interface INotification
    {
        /// <summary>The time at which the entity was created.</summary>
        DateTime Timestamp { get; set; }
    }
}
