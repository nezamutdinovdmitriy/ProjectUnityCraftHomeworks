using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;

namespace Game
{
    public sealed class TrampolineView : MonoBehaviour
    {
        private static readonly int ThrowUp = Animator.StringToHash(nameof(ThrowUp));
      
        [SerializeField]
        private TriggerComponent _triggerComponent;
        
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private Animator _animator;

        private Tween _tween;

        private void OnEnable() => _triggerComponent.OnEntered += this.OnEntered;

        private void OnDisable()
        {
            _triggerComponent.OnEntered -= this.OnEntered;
            _tween?.Kill();
        }

        [Button]
        private void OnEntered(Collider2D _)
        {
            _audioSource.Play();

            // если уже есть анимация — убиваем
            _tween?.Kill();

            _tween = DOTween.Sequence()
                .Append(DOTween.To(
                    () => _animator.GetFloat(ThrowUp),
                    x => _animator.SetFloat(ThrowUp, x),
                    1f,
                    0.1f // резко вверх
                ))
                .Append(DOTween.To(
                    () => _animator.GetFloat(ThrowUp),
                    x => _animator.SetFloat(ThrowUp, x),
                    0f,
                    0.15f // быстро вниз
                ))
                .Append(DOTween.To(
                    () => _animator.GetFloat(ThrowUp),
                    x => _animator.SetFloat(ThrowUp, x),
                    0.5f,
                    0.2f // немного вверх (bounce)
                ))
                .SetEase(Ease.OutQuad);
        }
    }
}