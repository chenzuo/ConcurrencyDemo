using PetClubLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetService
{
    internal class AsyncClientResult : AsyncResult<INotification>
    {
        public AsyncClientResult(AsyncCallback asyncCallback, object state, object owner, string operationId)
            : base(asyncCallback, state, owner, operationId)
        {
        }

        internal override void Process()
        {
 	         // Do stuff here
        }
    }
}
