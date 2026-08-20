using UnityEngine;

namespace SampleGame
{
    [ExecuteAlways]
    public sealed class TeamColorBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Renderer _meshRenderer;

        [SerializeField]
        private TeamConfig _teamConfig;

        [SerializeField]
        private TeamComponent _teamComponent;

        private void Update()
        {
            if (_teamConfig && _meshRenderer)
                _meshRenderer.material = _teamConfig.GetTeam(_teamComponent.Team).Material;
        }
    }
}