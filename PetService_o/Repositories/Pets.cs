using Dapper;
using PetClubLib.Models;
using PetService.DbEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PetService.Repositories
{
    public class Pets : RepositoryBase<PetModel>
    {
        public override long Insert(PetModel pet)
        {
            return SimpleCRUD.Insert<long>(_connection, ToDbo(pet));
        }

        public override void Update(PetModel pet)
        {
            // Get owner from cache
            SimpleCRUD.Update(_connection, ToDbo(pet));
        }

        public override PetModel Get(long id)
        {
            return ToModel(SimpleCRUD.Get<Pet>(_connection, id));
        }

        public override void Delete(long id)
        {
            SimpleCRUD.Delete<long>(_connection, id);
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