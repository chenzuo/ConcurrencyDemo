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
        public void AddPetOwner(PetOwner person)
        {
            r.PetOwners.Insert(person);
        }

        public void UpdatePetOwner(PetOwner person)
        {
            r.PetOwners.Update(person);
        }

        public PetOwner GetPetOwner(IAsyncResult async, long id)
        {
            var owner = r.PetOwners.Get(id);


            return r.PetOwners.Get(id);
        }

        public void DeletePetOwner(long id)
        {
            r.PetOwners.Delete(id);
        }

        public void AddPet(PetModel pet)
        {
            r.Pets.Insert(pet);
        }

        public void DeletePet(long id)
        {
            r.Pets.Delete(id);
        }

        void OnCacheChanged(object o, EventArgs e)
        {
            lastActivity = DateTime.Now;
        }

        internal ClientWorker(SessionKey key)
        {
            this.r = EntityRepository.Instance;
            lastActivity = DateTime.MinValue;
            r.PetOwners.HasChanged += OnCacheChanged;
            r.Pets.HasChanged += OnCacheChanged;
        }
        EntityRepository r;
        DateTime lastActivity;
    }
}
