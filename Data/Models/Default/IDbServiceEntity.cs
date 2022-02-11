namespace StepEbay.Data.Models.Default
{
    public interface IDbServiceEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
    }
}
