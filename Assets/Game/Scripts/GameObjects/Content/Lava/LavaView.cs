using UnityEngine;

namespace Game
{
    public sealed class LavaView : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip _audioClip;
        
        private TriggerComponent _triggerComponent;

        private void Awake() => _triggerComponent = this.GetComponentInParent<TriggerComponent>();

        private void OnEnable() => _triggerComponent.OnEntered += this.OnTrigger;

        private void OnDisable() => _triggerComponent.OnEntered -= this.OnTrigger;

        private void OnTrigger(Collider2D obj) => _audioSource.PlayOneShot(_audioClip);
    }
}