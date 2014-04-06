using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Linq;
using PetClubLib.Properties;

namespace PetClubLib.Models
{
    [DataContract]
    public class PetOwner : ModelBase
    {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Occupation { get; set; }
        [DataMember]
        public List<PetModel> Pets { get; set; }

        public override string ToString()
        {
            var z = new StringBuilder();
            if (Pets != null && Pets.Any())
            {
                z.AppendLine(string.Format(Resources.PetOwnerHasPets, Name, Occupation, Pets.Count));
                foreach (var p in Pets)
                    z.AppendLine("\t" + p);
            }
            else
            {
                z.AppendLine(string.Format(Resources.PetOwnerHasNoPets, Name, Occupation));
            }
            return z.ToString();
        }
    }
}
