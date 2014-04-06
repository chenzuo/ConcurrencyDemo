using PetClubLib.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PetClubLib.Models
{
    [DataContract]
    public class PetModel : ModelBase
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long OwnerId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Age { get; set; }
        [DataMember]
        public PetSpecies Species { get; set; }

        public override string ToString()
        {
            return string.Format(Resources.PetModel, Id, Name, Age, Species);
        }
    }

    [DataContract]
    public enum PetSpecies
    {
        [EnumMemberAttribute]
        Unknown,
        [EnumMemberAttribute]
        Dog,
        [EnumMemberAttribute]
        Cat,
        [EnumMemberAttribute]
        Fish,
        [EnumMemberAttribute]
        Bird
    }
}
