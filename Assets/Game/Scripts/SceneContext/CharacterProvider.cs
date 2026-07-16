using Game.Scripts.GameObjects;

namespace Game.Scripts.SceneContext
{
    public class CharacterProvider
    {
        private IEntity _character;

        public void Register(IEntity character) => _character = character;
        public void Unregister() => _character = null;

        public IEntity GetCharacter() => _character;
    }
}