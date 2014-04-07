using PetClubLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PetService
{
    [ServiceContract]
    public interface IPetService
    {
        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginGetPetOwner(AsyncCallback callback, object state);
        PetOwner EndGetPetOwner(IAsyncResult result);

        //[OperationContract(AsyncPattern = true)]
        //IAsyncResult BeginGetPet(AsyncCallback callback, object state);
        //PetModel EndGetPet(IAsyncResult result);

        [OperationContract(IsOneWay = true)]
        void UpdatePetOwner(Guid guid, PetOwner model);
        [OperationContract(IsOneWay = true)]
        void UpdatePetModel(Guid guid, PetModel model);
    }
}
