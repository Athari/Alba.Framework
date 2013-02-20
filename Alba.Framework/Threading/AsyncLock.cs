using System;
using System.Threading;
using System.Threading.Tasks;

namespace Alba.Framework.Threading
{
    public class AsyncLock
    {
        private readonly SemaphoreSlim _semaphore;

        public AsyncLock ()
        {
            _semaphore = new SemaphoreSlim(1, 1);
        }

        public async Task<IDisposable> LockAsync ()
        {
            await _semaphore.WaitAsync();
            return new Lock(this, true);
        }

        public async Task<IDisposable> LockAsync (TimeSpan timeout)
        {
            return new Lock(this, await _semaphore.WaitAsync(timeout));
        }

        private class Lock : ILock
        {
            private readonly AsyncLock _asyncLock;
            private readonly bool _isLocked;

            public Lock (AsyncLock asyncLock, bool isLocked)
            {
                _asyncLock = asyncLock;
                _isLocked = isLocked;
            }

            public void Dispose ()
            {
                if (IsLocked)
                    _asyncLock._semaphore.Release();
            }

            public bool IsLocked
            {
                get { return _isLocked; }
            }
        }
    }

    public interface ILock : IDisposable
    {
        bool IsLocked { get; }
    }
}