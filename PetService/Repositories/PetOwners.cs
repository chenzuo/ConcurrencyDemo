using Dapper;
using PetClubLib.Models;
using PetService.DbEntities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PetService.Repositories
{
    public class PetOwners : RepositoryBase<PetOwner>
    {
        public override long? Insert(PetOwner model)
        {
            long? dbId = SimpleCRUD.Insert<long?>(_connection, ToPerson(model));
            if (dbId != null)
            {
                UpdateCache(model, model.Id.ToString());
                return dbId;
            }
            return 0;
        }

        public override void Update(PetOwner model)
        {
            if (SimpleCRUD.Update(_connection, ToPerson(model)) > 0)
            {
                UpdateCache(Get(model.Id), model.Id.ToString());
            }
        }

        public override PetOwner Get(long id)
        {
            PetOwner model = GetCache(id.ToString());
            if (model == null)
            {
                var person = SimpleCRUD.Get<Person>(_connection, id);
                if (person != null)
                {
                    model = ToModel(person, GetPetsForOwner(id));
                    UpdateCache(model, model.Id.ToString());
                }
            }
            return model;
        }

        public List<PetOwner> GetList()
        {
            List<PetOwner> result = GetCache().ToList();
            if (!result.Any())
            {
                var persons = SimpleCRUD.GetList<Person>(_connection);
                if (persons != null && persons.Any())
                {
                    foreach (var person in persons)
                    {
                        var pets = GetPetsForOwner(person.Id);
                        var model = ToModel(person, pets);
                        result.Add(model);
                        UpdateCache(model, model.Id.ToString());
                    }
                }
            }
            return result;
        }

        public override void Delete(long id)
        {
            DeletePetsForOwner(id);
            SimpleCRUD.Delete<Person>(_connection, id);
            RemoveFromCache(new PetOwner(), id.ToString());
        }

        List<Pet> GetPetsForOwner(long id)
        {
            return _connection.Query<Pet>(@"SELECT OwnerID = @OwnerID", new { OwnerID = id }).ToList();
        }

        void DeletePetsForOwner(long id)
        {
            _connection.Execute(@"DELETE OwnerID = @OwnerID", new { OwnerID = id });
        }

        public PetOwner ToModel(Person person, IEnumerable<Pet> pets)
        {
            if (person == null) return null;
            return new PetOwner()
            {
                Id = person.Id,
                Name = person.Name,
                Occupation = person.Occupation,
                Pets = pets.Where(p => p != null).Select(p => _entities.Pets.ToModel(p)).ToList()
            };
        }

        public Person ToPerson(PetOwner model)
        {
            if (model == null) return null;
            return new Person()
            {
                Id = model.Id,
                Name = model.Name,
                Occupation = model.Occupation
            };
        }

        public PetOwners(IEntityRepository entities, IDbConnection connection) : base(entities, connection) { }
    }
}