using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PetService
{
    internal class AsyncResult<T> : AsyncResultNoResult
    {
        // Field set when operation completes

        private T result = default(T);

        protected void SetResult(T result)
        {
            this.result = result;
        }

        protected AsyncResult(AsyncCallback asyncCallback, object state, object owner, string operationId)
            : base(asyncCallback, state, owner, operationId)
        {
        }

        new public static T End(IAsyncResult result, object owner, string operationId)
        {
            AsyncResult<T> asyncResult = result as AsyncResult<T>;
            // Wait until operation has completed
            AsyncResultNoResult.End(result, owner, operationId);
            // Return the result (if above didn't throw)
            return asyncResult.result;
        }
    }

    internal class AsyncResultNoResult : IAsyncResult
    {
        #region IAsyncResult Implementation
        public object AsyncState
        {
            get { return asyncState; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                if (asyncWaitHandle == null)
                {
                    bool done = IsCompleted;
                    ManualResetEvent mre = new ManualResetEvent(done);
                    if (Interlocked.CompareExchange(ref asyncWaitHandle, mre, null) != null)
                    {
                        // Another thread created this object's event; dispose
                        // the event we just created
                        mre.Close();
                    }
                    else
                    {
                        if (!done && IsCompleted)
                        {
                            // If the operation wasn't done when we created
                            // the event but now it is done, set the event
                            asyncWaitHandle.Set();
                        }
                    }
                }
                return asyncWaitHandle;
            }
        }

        public bool CompletedSynchronously
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsCompleted
        {
            get { throw new NotImplementedException(); }
        }
        #endregion // IAsyncResult Implementation

        /// <summary>
        /// Start processing the operation.
        /// </summary>
        internal virtual void Process()
        {
        }

        protected bool Complete(Exception e = null, bool completedSynchronously = false)
        {
            bool result = false;

            // The completedState field MUST be set prior calling the callback
            var prevState = Interlocked.Exchange(ref completedState, completedSynchronously ?
                (int)ResultState.CompletedSynchronously : (int)ResultState.CompletedAsynchronously);

            if (prevState == (int)ResultState.Pending)
            {
                this.exception = e;
                //this.OnCompleting(e, completedSynchronously);
                if (asyncWaitHandle != null)
                    asyncWaitHandle.Set();
                if (asyncCallback != null)
                    asyncCallback(this);
                //this.OnCompleted(e, completedSynchronously);
                result = true;
            }

            return result;
        }

        public static void End(IAsyncResult result, object owner, string operationId)
        {
            var asyncResult = result as AsyncResultNoResult;

            if (asyncResult == null)
            {
                throw new ArgumentException("Result passed represents an operation not supported by this framework.", "result");
            }

            asyncResult.CheckUsage(owner, operationId);

            // This method assumes that only 1 thread calls EndInvoke
            // for this object
            if (!asyncResult.IsCompleted)
            {
                // If the operation isn't done, wait for it
                asyncResult.AsyncWaitHandle.WaitOne();
                asyncResult.AsyncWaitHandle.Close();
                asyncResult.asyncWaitHandle = null;  // Allow early GC
            }

            // Operation is done: if an exception occurred, throw it
            if (asyncResult.exception != null) throw asyncResult.exception;

        }

        void CheckUsage(object owner, string operationId)
        {
            if (!object.ReferenceEquals(owner, this.owner))
            {
                throw new InvalidOperationException("End was called on a different object than Begin.");
            }

            // Reuse the operation ID to detect multiple calls to end.
            if (object.ReferenceEquals(null, this.operationId))
            {
                throw new InvalidOperationException("End was called multiple times for this operation.");
            }

            if (!String.Equals(operationId, this.operationId))
            {
                throw new ArgumentException("End operation type was different than Begin.");
            }

            // Mark that End was already called.
            this.operationId = null;
        }

        protected AsyncResultNoResult(AsyncCallback asyncCallback, object state, object owner, string operationId)
        {
            this.asyncCallback = asyncCallback;
            this.asyncState = state;
            this.owner = owner;
            this.operationId = String.IsNullOrEmpty(operationId) ? "" : operationId;
            this.completedState = (int)ResultState.Pending;
        }

        readonly AsyncCallback asyncCallback;
        readonly Object asyncState;

        ManualResetEvent asyncWaitHandle;   // Field that may or may not get set depending on usage
        Exception exception;                // Fields set when operation completes
        object owner;                       // The object which started the operation.
        string operationId;                 // Used to verify BeginXXX and EndXXX calls match.
        int completedState;                 // Used to track the state of this operation

        protected enum ResultState
        {
            Pending,
            CompletedSynchronously,
            CompletedAsynchronously
        };
    }
}
