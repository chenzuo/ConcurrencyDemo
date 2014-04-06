using System;
using System.Runtime.Serialization;

namespace PetClubLib.Models
{
    [DataContract]
    public abstract class ModelBase : IModelMetadata
    {
        [DataMember]
        public DateTime Timestamp { get; set; }
    }

    public interface IModelMetadata
    {
        /// <summary>The time at which the entity was created.</summary>
        DateTime Timestamp { get; set; }
    }
}
