using PetConsole.PetService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PetConsole
{
    public class PetClient : ClientBase<IPetService>, IPetService
    {
        public PetClubLib.Models.PetOwner GetPetOwner()
        {
            throw new NotImplementedException();
        }

        public Task<PetClubLib.Models.PetOwner> GetPetOwnerAsync()
        {
            throw new NotImplementedException();
        }

        public PetClubLib.Models.PetModel GetPet()
        {
            throw new NotImplementedException();
        }

        public Task<PetClubLib.Models.PetModel> GetPetAsync()
        {
            throw new NotImplementedException();
        }
   } 
}
