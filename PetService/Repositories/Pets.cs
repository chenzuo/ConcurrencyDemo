using Dapper;
using PetClubLib.Models;
using PetService.DbEntities;
using System;
using System.Data;

namespace PetService.Repositories
{
    public class Pets : RepositoryBase<PetModel>
    {
        public override long? Insert(PetModel pet)
        {
            long? dbId = SimpleCRUD.Insert<long?>(_connection, ToDbo(pet));
            if (dbId != null)
            {
                UpdateCache(pet, pet.Id.ToString());
                return dbId;
            }
            return 0;
        }

        public override void Update(PetModel pet)
        {
            if (SimpleCRUD.Update(_connection, ToDbo(pet)) > 0)
            {
                UpdateCache(Get(pet.Id), pet.Id.ToString());
            }
        }

        public override PetModel Get(long id)
        {
            PetModel model = ToModel(SimpleCRUD.Get<Pet>(_connection, id));
            if (model != null)
            {
                UpdateCache(model, id.ToString());
            }
            return model;
        }

        public override void Delete(long id)
        {
            SimpleCRUD.Delete<long>(_connection, id);
            RemoveFromCache(new PetModel(), id.ToString());
        }

        public Pet ToDbo(PetModel model)
        {
            if (model == null) return null;
            return new Pet()
            {
                Age = model.Age,
                Name = model.Name,
                OwnerId = model.OwnerId,
                Id = model.Id,
                Species = (int)model.Species
            };
        }

        public PetModel ToModel(Pet dbo)
        {
            if (dbo == null) return null;
            return new PetModel()
            {
                Age = dbo.Age,
                Name = dbo.Name,
                Id = dbo.Id,
                Species = Enum.IsDefined(typeof(PetSpecies), dbo.Species) ? (PetSpecies)dbo.Species : PetSpecies.Unknown
            };
        }

        public Pets(IEntityRepository entities, IDbConnection connection) : base(entities, connection) { }
    }
}