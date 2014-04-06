using PetService.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
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
        public void GetPerson(Guid guid, long id)
        {
            throw new NotImplementedException();
        }

        public void GetPet(Guid guid, long id)
        {
            throw new NotImplementedException();
        }

        public void RegisterClient(Guid guid)
        {
            if (!Clients.ContainsKey(guid))
                Clients.Add(guid, new ClientWorker(guid));
        }

        public void UnregisterClient(Guid guid)
        {
            if (Clients.ContainsKey(guid))
                Clients.Remove(guid);
        }

        internal PetService()
        {
            Clients = new Dictionary<Guid, ClientWorker>();
            var guid = Guid.NewGuid();
            Clients.Add(guid, new ClientWorker(guid));
        }
        Dictionary<Guid, ClientWorker> Clients { get; set; }
    }
}
