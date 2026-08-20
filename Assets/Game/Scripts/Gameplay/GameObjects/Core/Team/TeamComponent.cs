using System;
using UnityEngine;

namespace SampleGame
{
    public sealed class TeamComponent : MonoBehaviour
    {
        public event Action<TeamType> OnTeamChanged;

        [SerializeField]
        private TeamType _team;

        public TeamType Team
        {
            get { return _team; }

            set
            {
                if (_team != value)
                {
                    _team = value;
                    this.OnTeamChanged?.Invoke(_team);
                }
            }
        }

        public bool IsEnemy(GameObject other) =>
            other.TryGetComponent(out TeamComponent teamComponent) && teamComponent._team != _team;
    }
}