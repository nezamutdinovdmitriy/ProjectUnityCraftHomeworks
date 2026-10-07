using Game.Components;
using Game.UseCases;
using Unity.Entities;

namespace Game.Predicates
{
    public struct IsEnemyPredicate : ITargetPredicate
    {
        private readonly Entity _self;
        private readonly TeamType _selfTeam;

        private ComponentLookup<Team> _teamLookup;
        private ComponentLookup<Health> _healthLookup;

        public IsEnemyPredicate(
            Entity self,
            TeamType selfTeam,
            ComponentLookup<Team> teamLookup,
            ComponentLookup<Health> healthLookup)
        {
            _self = self;
            _selfTeam = selfTeam;
            _teamLookup = teamLookup;
            _healthLookup = healthLookup;
        }

        public bool Invoke(Entity candidate)
        {
            return candidate != _self
                   && _teamLookup.TryGetComponent(candidate, out Team team)
                   && team.Value != _selfTeam
                   && _healthLookup.TryGetComponent(candidate, out Health health)
                   && health.IsAlive();
        }
    }
}