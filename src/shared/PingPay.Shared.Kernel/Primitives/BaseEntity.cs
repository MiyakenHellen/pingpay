namespace PingPay.Shared.Kernel.Primitives
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; protected set; }

        public void SetUpdatedAt() => UpdatedAt = DateTime.UtcNow;
    }
}
