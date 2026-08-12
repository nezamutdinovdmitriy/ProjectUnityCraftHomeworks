using Atomic.Entities;

namespace Game.GameEntities
{
    public class BulletEntityPool : SceneEntityPool<GameEntity>, IEntityPool<IGameEntity>
    {
        IGameEntity IEntityPool<IGameEntity>.Rent() => Rent();

        void IEntityPool<IGameEntity>.Return(IGameEntity entity) => Return((GameEntity)entity);

        protected override void OnRent(GameEntity entity)
        {
        }

        protected override void OnReturn(GameEntity entity)
        {
        }
    }
}