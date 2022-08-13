using System.Collections.Concurrent;

namespace StepEbay.Main.Api.Common.Models.HubContainers
{
    public class HubUserContainer
    {
        /// <summary>
        ///    Користувачі підключені до хаба
        /// </summary>
        public ConcurrentDictionary<string, int> Users { get; } = new();
    }
}
