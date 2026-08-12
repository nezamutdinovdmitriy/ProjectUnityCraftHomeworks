using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntity.Core.Target;
using UnityEngine;

namespace Game.GameEntity
{
    public class EnemyTriggerInstaller : SceneEntityInstaller<IGameEntity>
    {
        [SerializeField]
        private TriggerEvents _triggerEvents;

        [SerializeField]
        private GameEntity[] _enemies;

        public override void Install(IGameEntity entity)
        {
            entity.AddValue(GameEntityAPI.Trigger, _triggerEvents);
            entity.AddBehaviour(new DetectTargetBehaviour(_enemies));
        }
    }
}