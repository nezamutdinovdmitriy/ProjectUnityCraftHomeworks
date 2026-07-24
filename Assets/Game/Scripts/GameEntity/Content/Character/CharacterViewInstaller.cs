using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.GameEntity.Content.Character
{
    public class CharacterViewInstaller : SceneEntityInstaller<IGameEntity>
    {
        private readonly int IsMovingKey = Animator.StringToHash("IsMoving");
        
        private readonly DisposableComposite _disposables = new();
        
        [SerializeField]
        private Animator _animator;
        
        public override void Install(IGameEntity entity)
        {
            entity.GetValue(GameEntityAPI.IsMoving).Subscribe(OnMoved).AddTo(_disposables);
        }

        private void OnMoved(bool isMoving)
        {
            _animator.SetBool(IsMovingKey, isMoving);
        }

        public override void Uninstall(IGameEntity entity)
        {
            _disposables.Dispose();
        }
    }
}