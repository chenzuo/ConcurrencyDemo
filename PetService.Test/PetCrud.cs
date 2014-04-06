using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetService.Repositories;
using System.Linq;
using PetClubLib.Models;

namespace PetService.Test
{
    [TestClass]
    public class PetCrud
    {
        [TestInitialize]
        public void Initialize()
        {
            _entities = new EntityRepository();

        }

        [TestMethod]
        public void AddThenUpdatePerson()
        {
            var person = new PetOwner()
            {
                Name = "Bart Sampson",
                Occupation = "Student"
            };
            var id = _entities.PetOwners.Insert(person);
            Assert.IsTrue(id != null);
            person.Id = id.GetValueOrDefault();
            person.Name = "Bart Simpson";
            _entities.PetOwners.Update(person);
            var result = _entities.PetOwners.Get(person.Id);
            Assert.AreEqual(result.Name, person.Name);
            Assert.AreEqual(result.Occupation, person.Occupation);
        }

        [TestMethod]
        public void AddPetForPerson()
        {
            var owner = _entities.PetOwners.GetList().LastOrDefault();
            Assert.IsNotNull(owner);
            var id = _entities.Pets.Insert(new PetModel()
            {
                Name = "Snowball",
                Age = 3,
                OwnerId = owner.Id,
                Species = PetSpecies.Cat
            });
            Assert.IsTrue(id > 0);
        }

        EntityRepository _entities;
    }
}
