using UnityEngine;
using Zenject;

namespace Game
{
    public class CharacterView : MonoBehaviour
    {
        [SerializeField]
        private AttackComponentView _pushView;
        
        [SerializeField]
        private AttackComponentView _tossView;
        
        [Inject]
        public void Construct(
            [Inject(Id = AttackType.Push)] AttackRequestComponent pushAttackRequestComponent, 
            [Inject(Id = AttackType.Toss)] AttackRequestComponent tossAttackRequestComponent)
        {
            _pushView.Construct(pushAttackRequestComponent);
            _tossView.Construct(tossAttackRequestComponent);
        }
    }
}