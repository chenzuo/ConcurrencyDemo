using PetService.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PetService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = true)]
    public class PetService : IPetService
    {
        internal PetService()
        {
            Clients = new Dictionary<Guid, ClientWorker>();
        }
        Dictionary<Guid, ClientWorker> Clients { get; set; }

        public IAsyncResult BeginGetPetOwner(AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public PetClubLib.Models.PetOwner EndGetPetOwner(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public void UpdatePetOwner(Guid guid, PetClubLib.Models.PetOwner model)
        {
            throw new NotImplementedException();
        }

        public void UpdatePetModel(Guid guid, PetClubLib.Models.PetModel model)
        {
            throw new NotImplementedException();
        }
    }

    //public class NotificationResult : LongPollingAsyncResult<string>
    //{
    //}
}
