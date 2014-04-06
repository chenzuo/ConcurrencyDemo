using PetClubLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PetService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IPetService
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetPetOwner(AsyncCallback callback, object state);
        PetOwner EndGetPetOwner(IAsyncResult result);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetPet(AsyncCallback callback, object state);
        PetModel EndGetPet(IAsyncResult result);

        //[OperationContract]
        //void UpdatePerson(Guid guid, Patient patient);
        //[OperationContract]
        //void UpdatePet(Guid guid, Pet script);

        // TODO: Add your service operations here
    }
}
