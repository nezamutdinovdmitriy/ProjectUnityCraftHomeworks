using Atomic.Entities;

namespace Game.GameEntity
{
    public class GameEntityPool : SceneEntityPool<GameEntity>, IEntityPool<IGameEntity>
    {
        IGameEntity IEntityPool<IGameEntity>.Rent() => Rent();

        void IEntityPool<IGameEntity>.Return(IGameEntity entity) => Return((GameEntity)entity);
    }
}