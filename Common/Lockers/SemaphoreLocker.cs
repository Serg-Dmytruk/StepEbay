namespace StepEbay.Common.Lockers
{
    public class SemaphoreLocker
    {
        private readonly SemaphoreSlim _semaphore;

        public SemaphoreLocker(int number)
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
