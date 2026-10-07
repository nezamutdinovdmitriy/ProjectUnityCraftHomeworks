using Game.Components;
using Game.UseCases;
using Unity.Entities;

namespace Game.Predicates
{
    public struct IsWoundedAllyPredicate : ITargetPredicate
    {
        private readonly Entity _self;
        private readonly TeamType _selfTeam;

        private ComponentLookup<Team> _teamLookup;
        private ComponentLookup<Health> _healthLookup;
        private ComponentLookup<MaxHealth> _maxHealthLookup;

        public IsWoundedAllyPredicate(
            Entity self, 
            TeamType selfTeam,
            ComponentLookup<Team> teamLookup,
            ComponentLookup<Health> healthLookup,
            ComponentLookup<MaxHealth> maxHealthLookup)
        {
            _self = self;
            _selfTeam = selfTeam;
            _teamLookup = teamLookup;
            _healthLookup = healthLookup;
            _maxHealthLookup = maxHealthLookup;
        }

        public bool Invoke(Entity candidate)
        {
            return candidate != _self
                   && _teamLookup.TryGetComponent(candidate, out Team team)
                   && team.Value == _selfTeam
                   && _healthLookup.TryGetComponent(candidate, out Health health)
                   && health.IsAlive()
                   && _maxHealthLookup.TryGetComponent(candidate, out MaxHealth maxHealth)
                   && health.Value < maxHealth.Value;
        }
    }
}