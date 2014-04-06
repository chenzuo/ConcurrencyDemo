using Dapper;
using PetClubLib.Models;
using PetService.DbEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PetService.Repositories
{
    public class PetOwners : RepositoryBase<PetOwner>
    {
        public override long Insert(PetOwner model)
        {
            return SimpleCRUD.Insert<long>(_connection, ToPerson(model));
        }

        public override void Update(PetOwner model)
        {
            SimpleCRUD.Update(_connection, ToPerson(model));
        }

        public override PetOwner Get(long id)
        {
            var person = SimpleCRUD.Get<Person>(_connection, id);
            if (person != null)
            {
                var pets = GetPetsForOwner(id);
                return ToModel(person, pets);
            }
            return null;
        }

        public override void Delete(long id)
        {
            DeletePetsForOwner(id);
            SimpleCRUD.Delete<Person>(_connection, id);
        }

        public List<Pet> GetPetsForOwner(long id)
        {
            return _connection.Query<Pet>(@"SELECT from dbo.Pet WHERE OwnerID = @OwnerID", new { OwnerID = id }).ToList();
        }

        public void DeletePetsForOwner(long id)
        {
            _connection.Execute(@"DELETE from dbo.Pet WHERE OwnerID = @OwnerID", new { OwnerID = id });
        }

        public PetOwner ToModel(Person person, IEnumerable<Pet> pets)
        {
            if (person == null) return null;
            return new PetOwner()
            {
                Name = person.Name,
                Occupation = person.Occupation,
                Pets = pets.Where(p => p != null).Select(p => _entities.Pets.ToModel(p)).ToList()
            };
        }

        public PetOwner ToPerson(PetOwner model)
        {
            if (model == null) return null;
            return new PetOwner()
            {
                Name = model.Name,
                Occupation = model.Occupation
            };
        }

        public PetOwners(IEntityRepository entities, IDbConnection connection) : base(entities, connection) { }
    }
}