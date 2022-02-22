using System.Collections.Concurrent;

namespace StepEbay.Common.Lockers
{
    public class SemaphoreManager
    {
        public ConcurrentDictionary<string, SemaphoreLocker> Lockers = new();

        public SemaphoreLocker Get(string key)
        {
            if (!Lockers.ContainsKey(key))
                Lockers.TryAdd(key, new SemaphoreLocker(1));

            return Lockers[key];
        }
    }
}
