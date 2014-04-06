using PetService.Properties;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetService.DbEntities
{
    [Table("Person")]
    public class Person
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Occupation { get; set; }

        public override string ToString()
        {
            return string.Format(Resources.Person, Id, Name, Occupation);
        }
    }
}
