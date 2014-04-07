using System;
using System.Runtime.Serialization;

namespace PetClubLib.Models
{
    [DataContract]
    public class SessionKey
    {
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public Guid Guid { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var otherKey = obj as SessionKey;
            return (otherKey != null
                && this.UserName == otherKey.UserName
                && this.Guid == otherKey.Guid);
        }

        public override int GetHashCode()
        {
            return (Guid.GetHashCode() * 2 + 5) * UserName.GetHashCode();
        }
    }
}
