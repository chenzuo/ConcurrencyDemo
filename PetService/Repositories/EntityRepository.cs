using PetService.Access;
using System.Data;
using System.Data.SqlClient;

namespace PetService.Repositories
{
    public class EntityRepository : IEntityRepository
    {
        public static EntityRepository Instance
        {
            get
            {
                return (_instance) ?? (_instance = new EntityRepository());
            }
        }
        static EntityRepository _instance;

        public PetOwners PetOwners
        {
            get { return (_petOwners) ?? (_petOwners = new PetOwners(this, _connection)); }
        }

        public Pets Pets
        {
            get { return (_pets) ?? (_pets = new Pets(this, _connection)); }
        }

        EntityRepository()
        {
            _connection = new SqlConnection(DbAccess.ConnectionString);
        }

        PetOwners _petOwners;
        Pets _pets;
        IDbConnection _connection;
    }

    public interface IEntityRepository
    {
        PetOwners PetOwners { get; }
        Pets Pets { get; }
    }
}
