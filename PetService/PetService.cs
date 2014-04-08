using PetClubLib.Models;
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
            Clients = new Dictionary<SessionKey, ClientWorker>();
        }
        Dictionary<SessionKey, ClientWorker> Clients { get; set; }

        public IAsyncResult BeginGetPetOwner(AsyncCallback callback, object state)
        {
            return new AsyncClientResult(callback, state, this, "GetPetOwner");
        }

        public PetOwner EndGetPetOwner(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public void AddPetOwner(SessionKey key, PetOwner model)
        {
            if (Clients.ContainsKey(key))
            {
                Clients[key].AddPetOwner(model);
            }
        }

        public void UpdatePetOwner(SessionKey key, PetOwner model)
        {
            if (Clients.ContainsKey(key))
            {
                Clients[key].UpdatePetOwner(model);
            }
        }

        public void DeletePetOwner(SessionKey key, long id)
        {
            if (Clients.ContainsKey(key))
            {
                Clients[key].DeletePetOwner(id);
            }
        }

        public void UpdatePetModel(SessionKey key, PetModel model)
        {
            throw new NotImplementedException();
        }

        public SessionKey RegisterClient(string user)
        {
            var key = new SessionKey() { Guid = Guid.NewGuid(), UserName = user };
            Clients.Add(key, new ClientWorker(key));
            return key;
        }

        public void UnregisterClient(SessionKey key)
        {
            if (Clients.ContainsKey(key))
                Clients.Remove(key);
        }
    }

    //public class NotificationResult : LongPollingAsyncResult<string>
    //{
    //}
}
