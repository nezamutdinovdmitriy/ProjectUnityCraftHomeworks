using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntity;

namespace Game.UI
{
    public class CharacterKillsPresenter : IUIContextInit, IUIContextDispose
    {
        private readonly DisposableComposite _disposables = new();
        private readonly IGameEntity _character;
        
        private StatView _view;

        public CharacterKillsPresenter(IGameEntity character) 
            => _character = character;

        public void Init(IUIContext context)
        {
            _view = context.GetValue(UIContextAPI.KillsView);

            IReactiveVariable<int> score = _character.GetValue(GameEntityAPI.Score);
            score.Observe(OnScoreChanged);
        }

        public void Dispose(IUIContext context) => _disposables?.Dispose();
        
        private void OnScoreChanged(int value) => _view.SetText(value.ToString());
    }
}