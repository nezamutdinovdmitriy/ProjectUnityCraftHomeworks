using GameObjects.Components;

namespace GameContexts
{
    public class CharacterProvider
    {
        private readonly IEntity _character;

        public CharacterProvider(IEntity character)
            => _character = character;

        public IEntity GetCharacter() => _character;
    }
}