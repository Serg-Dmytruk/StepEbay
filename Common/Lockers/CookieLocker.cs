﻿namespace StepEbay.Common.Lockers
{
    public class CookieLocker
    {
        private readonly SemaphoreSlim _semaphore;

        public CookieLocker(int number)
        {
            _semaphore = new SemaphoreSlim(number, number);
        }

        public async Task LockAsync(Func<Task> worker)
        {
            await _semaphore.WaitAsync();

            try
            {
                await worker();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}

