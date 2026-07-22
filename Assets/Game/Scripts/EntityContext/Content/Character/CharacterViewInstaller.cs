using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.EntityContext
{
    public class CharacterViewInstaller : SceneEntityInstaller<IEntityContext>
    {
        private readonly int IsMovingKey = Animator.StringToHash("IsMoving");
        private readonly int IsDeathKey = Animator.StringToHash("Death");
        
        [SerializeField]
        private Animator _animator;
        
        private readonly DisposableComposite<Subscription<bool>> _disposables = new();

        public override void Install(IEntityContext entity)
        {
            entity.GetValue(EntityContextAPI.IsDead).Subscribe(OnDeath).AddTo(_disposables);

            entity.WhenFixedTick(_ =>
            {
                _animator.SetBool(IsMovingKey, entity.GetValue(EntityContextAPI.IsMoving).Value);
            });
        }

        public void TestMethod()
        {
            
        }
        
        public override void Uninstall(IEntityContext entity)
        {
            _disposables.Dispose();
        }

        private void OnDeath(bool isDeath)
        {
            Debug.Log("OnDeath method invoke");
            if (isDeath)
            {
                Debug.Log("Is Death true");
                _animator.SetTrigger(IsDeathKey);
            }
        }
    }
}