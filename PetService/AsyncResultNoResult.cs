using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PetService
{
    internal partial class AsyncResultNoResult : IAsyncResult
    {
        // Fields set at construction which never change while
        // operation is pending
        readonly AsyncCallback asyncCallback;
        readonly Object asyncState;

        // Fields set at construction which do change after

        // operation completes

        enum ResultState
        {
            Pending,
            CompletedSynchronously,
            CompletedAsynchronously
        };

        ResultState completedState = ResultState.Pending;

        // Field that may or may not get set depending on usage
        ManualResetEvent m_AsyncWaitHandle;

        // Fields set when operation completes
        Exception m_exception;

        /// <summary>The object which started the operation.</summary>
        object owner;

        /// <summary>Used to verify BeginXXX and EndXXX calls match.</summary>
        private string operationId;

        protected AsyncResultNoResult(AsyncCallback asyncCallback, object state, object owner, string operationId)
        {
            this.asyncCallback = asyncCallback;
            this.asyncState = state;
            this.owner = owner;
            this.operationId = String.IsNullOrEmpty(operationId) ? String.Empty : operationId;
        }
        
        internal virtual void Process()
        {

            // Starts processing of the operation.

        }
        
        protected bool Complete(Exception exception)
        {
            return this.Complete(exception, false /*completedSynchronously*/);
        }

        protected bool Complete(Exception exception, bool completedSynchronously)
        {

            bool result = false;

            // The completedState field MUST be set prior calling the callback
            var prevState = Interlocked.Exchange(ref completedState, completedSynchronously ?
                ResultState.CompletedSynchronously : ResultState.CompletedAsynchronously);

            if (prevState == ResultState.Pending)
            {
                // Passing null for exception means no error occurred.
                // This is the common case
                m_exception = exception;

                // Do any processing before completion.
                this.Completing(exception, completedSynchronously);
                
                // If the event exists, set it
                if (m_AsyncWaitHandle != null) m_AsyncWaitHandle.Set();

                this.MakeCallback(asyncCallback, this);

                // Do any final processing after completion
                this.Completed(exception, completedSynchronously);

                result = true;
            }

            return result;
        }

        private void CheckUsage(object owner, string operationId)
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

        public static void End(IAsyncResult result, object owner, string operationId)
        {
            AsyncResultNoResult asyncResult = result as AsyncResultNoResult;

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

                asyncResult.m_AsyncWaitHandle = null;  // Allow early GC

            }

            // Operation is done: if an exception occurred, throw it
            if (asyncResult.m_exception != null) throw asyncResult.m_exception;

        }

        #region Implementation of IAsyncResult
        public Object AsyncState { get { return asyncState; } }

        public bool CompletedSynchronously
        {
            get { return Thread.VolatileRead(ref completedState) == ResultState.CompletedSynchronously; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                if (m_AsyncWaitHandle == null)
                {
                    bool done = IsCompleted;
                    ManualResetEvent mre = new ManualResetEvent(done);
                    if (Interlocked.CompareExchange(ref m_AsyncWaitHandle, mre, null) != null)
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
                            m_AsyncWaitHandle.Set();
                        }
                    }
                }
                return m_AsyncWaitHandle;
            }
        }

        public bool IsCompleted
        {
            get { return Thread.VolatileRead(ref completedState) != ResultState.Pending; }
        }

        #endregion

        #region Extensibility
        protected virtual void Completing(Exception exception, bool completedSynchronously)
        {
        }

        protected virtual void MakeCallback(AsyncCallback callback, AsyncResultNoResult result)
        {
            // If a callback method was set, call it
            if (callback != null)
            {
                callback(result);
            }
        }

        protected virtual void Completed(Exception exception, bool completedSynchronously)
        {
        }
        #endregion
    }
}
