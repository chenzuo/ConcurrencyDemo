using System;
using System.Diagnostics;
using System.Threading;

namespace PetService
{
    /// <summary>
    /// Abstract base class for an asynchronous operation that periodically checks for data, returning the data on success and timing out after a certain time period
    /// </summary>
    /// <typeparam name="TResult">The expected result type of the asynchronous operation</typeparam>
    /// <author>banderton</author>
    public abstract class LongPollingAsyncResult<TResult> : IAsyncResult where TResult : class
    {
        #region - Fields -

        private AsyncCallback _callback;
        private TimeSpan _timoutSpan;
        private TimeSpan _intervalWaitSpan;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the exception that was thrown by the asynchronous operation, if any
        /// </summary>
        /// <author>banderton</author>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Gets the final result of the asynchronous operation
        /// </summary>
        /// <author>banderton</author>
        public TResult Result { get; private set; }

        /// <summary>
        /// Gets an object that can be used to lock the object for thread safety
        /// </summary>
        /// <author>banderton</author>
        public object SyncRoot { get; private set; }

        #endregion

        #region - Ctor -

        /// <summary>
        /// Creates a new <see cref="LongPollingAsyncResult{TResult}"/> object
        /// </summary>
        /// <param name="callback">the method to call after the operation is complete</param>
        /// <param name="asyncState">the object to pass along to the callback method for state information</param>
        /// <param name="timeoutSeconds">the number of seconds to wait before the async operation times out</param>
        /// <param name="intervalWaitMilliseconds">the number of milliseconds to wait before each work attempt</param>
        /// <author>banderton</author>
        public LongPollingAsyncResult(AsyncCallback callback, object asyncState, int timeoutSeconds = 300, int intervalWaitMilliseconds = 500)
        {
            SyncRoot = new object();
            _callback = callback;
            AsyncState = asyncState;
            AsyncWaitHandle = new ManualResetEvent(IsCompleted);
            _timoutSpan = TimeSpan.FromSeconds(timeoutSeconds);
            _intervalWaitSpan = TimeSpan.FromMilliseconds(intervalWaitMilliseconds);

            ThreadPool.QueueUserWorkItem(new WaitCallback(LoopWithIntervalAndTimeout));
        }

        #endregion

        #region - Private Helper Methods -

        private void LoopWithIntervalAndTimeout(object input)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while (!IsCompleted)
                {
                    if (stopwatch.Elapsed > _timoutSpan)
                        throw new TimeoutException();

                    DoWork();

                    if (!IsCompleted)
                        Thread.Sleep(_intervalWaitSpan);
                }
            }
            catch (Exception e)
            {
                Complete(null, e);
            }
        }

        #endregion

        #region - Protected/Abstract Methods -

        /// <summary>
        /// Completes the asynchronous loop
        /// </summary>
        /// <param name="result">the result to pass on to the client</param>
        /// <param name="e">an exception to re-throw to the client</param>
        /// <param name="completedSynchronously">whether the operation was completed synchronously</param>
        /// <author>banderton</author>
        protected void Complete(TResult result, Exception e = null, bool completedSynchronously = false)
        {
            lock (SyncRoot)
            {
                CompletedSynchronously = completedSynchronously;
                Result = result;
                Exception = e;
                IsCompleted = true;

                if (_callback != null)
                    _callback(this);
            }
        }

        /// <summary>
        /// Performs loop actions to check for data to return, should call <see cref="Complete"/> once data is found
        /// </summary>
        /// <author>banderton</author>
        protected abstract void DoWork();

        #endregion

        #region - Public Methods -

        /// <summary>
        /// Waits for the asynchronous operation to complete or timeout, returning the result or throwing the exception
        /// </summary>
        /// <returns>the result of the asynchronous operation</returns>
        /// <author>banderton</author>
        public TResult WaitForResult()
        {
            if (!IsCompleted)
                AsyncWaitHandle.WaitOne();

            if (Exception is TimeoutException)
            {
                throw Exception;
            }

            return Result;
        }

        #endregion

        #region - IAsyncResult Implementation -

        /// <summary>
        /// Gets a user-defined object that qualifies or contains information about an asynchronous operation.
        /// </summary>
        /// <author>banderton</author>
        public object AsyncState { get; private set; }

        /// <summary>
        /// Gets a <see cref="WaitHandle"/> that is used to wait for an asynchronous operation to complete.
        /// </summary>
        /// <author>banderton</author>
        public WaitHandle AsyncWaitHandle { get; private set; }

        /// <summary>
        /// Gets a value that indicates whether the asynchronous operation completed synchronously.
        /// </summary>
        /// <author>banderton</author>
        public bool CompletedSynchronously { get; private set; }

        /// <summary>
        /// Gets a value that indicates whether the asynchronous operation has completed.
        /// </summary>
        /// <author>banderton</author>
        public bool IsCompleted { get; private set; }

        #endregion
    }
}