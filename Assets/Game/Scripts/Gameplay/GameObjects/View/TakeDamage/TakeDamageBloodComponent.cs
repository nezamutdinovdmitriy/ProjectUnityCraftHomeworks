using UnityEngine;

namespace SampleGame
{
    public sealed class TakeDamageBloodComponent : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _bloodVfx;

        [SerializeField]
        private TeamConfig _teamConfig;

        [SerializeField]
        private TakeDamageComponent _takeDamageComponent;

        [SerializeField]
        private TeamComponent _teamComponent;

        private void OnEnable()
        {
            _takeDamageComponent.OnDamageTaken += this.OnDamageTaken;
        }

        private void OnDisable()
        {
            _takeDamageComponent.OnDamageTaken += this.OnDamageTaken;
        }
        
        private void OnDamageTaken(TakeDamageArgs obj)
        {
            Color color = _teamConfig.GetTeam(_teamComponent.Team).Material.color;
            foreach (ParticleSystem particle in _bloodVfx.GetComponentsInChildren<ParticleSystem>())
            {
                ParticleSystem.MainModule particleMain = particle.main;
                particleMain.startColor = color;
            }

            _bloodVfx.Play(withChildren: true);
        }
    }
}