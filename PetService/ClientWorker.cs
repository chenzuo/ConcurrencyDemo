using PetClubLib.Models;
using PetService.DbEntities;
using PetService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetService
{
    internal class ClientWorker
    {
        public DateTime LastActivity { get; private set; }

        public void AddPetOwner(PetOwner person)
        {
            _entities.PetOwners.Insert(person);
        }

        public PetOwner GetPetOwner(long id)
        {
            return _entities.PetOwners.Get(id);
        }

        public void DeletePetOwner(long id)
        {
            _entities.PetOwners.Delete(id);
        }

        public void AddPet(PetModel pet)
        {
            _entities.Pets.Insert(pet);
        }

        public void DeletePet(long id)
        {
            _entities.Pets.Delete(id);
        }

        internal ClientWorker(Guid guid)
        {
            this.clientID = guid;
            this._entities = new EntityRepository();
            this.LastActivity = DateTime.MinValue;
        }
        EntityRepository _entities;
        Guid clientID;
    }
}
