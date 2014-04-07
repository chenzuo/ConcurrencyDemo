using PetClubLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PetService
{
    class AsyncWorkerResult<T> : IAsyncResult where T : class, INotification
    {
        public object AsyncState
        {
            get { throw new NotImplementedException(); }
        }

        public System.Threading.WaitHandle AsyncWaitHandle
        {
            get { throw new NotImplementedException(); }
        }

        public bool CompletedSynchronously
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsCompleted
        {
            get { throw new NotImplementedException(); }
        }

        public AsyncWorkerResult(AsyncCallback asyncCallback, object state, object owner, string operationId)
        {
            this.asyncCallback = asyncCallback;
            this.asyncState = state;
            this.owner = owner;
            this.operationId = string.IsNullOrEmpty(operationId) ? "" : operationId;
        }

        readonly AsyncCallback asyncCallback;
        readonly object asyncState;

        ManualResetEvent waitHandle;
        Exception exception;
        object owner;
        string operationId;
    }
}
