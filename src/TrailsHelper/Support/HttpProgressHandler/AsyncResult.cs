// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Threading;

namespace TrailsHelper.Support.HttpProgressHandler
{
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "_manualResetEvent is disposed in End<TAsyncResult>")]
    internal abstract class AsyncResult : IAsyncResult
    {
        private AsyncCallback _callback;
        private object _state;

        private bool _isCompleted;
        private bool _completedSynchronously;
        private bool _endCalled;

        private Exception _exception;

        protected AsyncResult(AsyncCallback callback, object state)
        {
            _callback = callback;
            _state = state;
        }

        public object AsyncState
        {
            get { return _state; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                Contract.Assert(false, "AsyncWaitHandle is not supported -- use callbacks instead.");
                return null;
            }
        }

        public bool CompletedSynchronously
        {
            get { return _completedSynchronously; }
        }

        public bool HasCallback
        {
            get { return _callback != null; }
        }

        public bool IsCompleted
        {
            get { return _isCompleted; }
        }

        protected void Complete(bool completedSynchronously)
        {
            if (_isCompleted)
            {
                throw new InvalidOperationException($"multiple completes: {GetType().Name}");
            }

            _completedSynchronously = completedSynchronously;
            _isCompleted = true;

            if (_callback != null)
            {
                try
                {
                    _callback(this);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("callback threw and exception", e);
                }
            }
        }

        protected void Complete(bool completedSynchronously, Exception exception)
        {
            _exception = exception;
            Complete(completedSynchronously);
        }

        protected static TAsyncResult End<TAsyncResult>(IAsyncResult result) where TAsyncResult : AsyncResult
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            TAsyncResult thisPtr = result as TAsyncResult;

            if (thisPtr == null)
            {

                throw new ArgumentException("The result was mismatched", "result");
            }

            if (!thisPtr._isCompleted)
            {
                thisPtr.AsyncWaitHandle.WaitOne();
            }

            if (thisPtr._endCalled)
            {
                throw new InvalidOperationException("end was called multiple times");
            }

            thisPtr._endCalled = true;

            if (thisPtr._exception != null)
            {
                throw thisPtr._exception;
            }

            return thisPtr;
        }
    }
}