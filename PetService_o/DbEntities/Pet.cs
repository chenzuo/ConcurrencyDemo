using PetService.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetService.DbEntities
{
    [Table("Pet")]
    public class Pet
    {
        [Key]
        public long Id { get; set; }
        public long OwnerId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Species { get; set; }

        public override string ToString()
        {
            return string.Format(Resources.Pet, Id, Name, Age, Species, OwnerId);
        }
    }
}
