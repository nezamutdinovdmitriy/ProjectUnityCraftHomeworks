namespace Game
{
    public interface IDamageable
    {
        public TeamType Team { get; }
        public void TakeDamage(int damage);
    }
}