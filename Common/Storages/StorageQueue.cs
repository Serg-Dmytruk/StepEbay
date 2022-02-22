namespace StepEbay.Common.Storages
{
    public class StorageQueue
    {
        public bool Ready { get; set; }
        public Queue<Func<Task>> Tasks { get; set; } = new();
    }
}
