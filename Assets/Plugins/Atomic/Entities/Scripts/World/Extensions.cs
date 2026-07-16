using System.Runtime.CompilerServices;

namespace Atomic.Entities
{
    public partial class Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BindTo<E>(this IEntityWorld<E> world, IEntity entity) where E : IEntity
        {
            entity.WhenEnable(world.Enable);
            entity.WhenDisable(world.Disable);
            entity.WhenTick(world.Tick);
            entity.WhenFixedTick(world.FixedTick);
            entity.WhenLateTick(world.LateTick);
        }
    }
}