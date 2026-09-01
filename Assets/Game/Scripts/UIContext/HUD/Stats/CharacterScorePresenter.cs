using Atomic.Elements;
using Atomic.Entities;

namespace Game.UI
{
    public class CharacterScorePresenter : IUIContextInit, IUIContextDispose
    {
        private readonly DisposableComposite _disposables = new();
        private readonly GameContext _gameContext;
        
        private StatView _view;

        public CharacterScorePresenter(GameContext gameContext) 
            => _gameContext = gameContext;

        public void Init(IUIContext context)
        {
            _view = context.GetValue(UIContextAPI.KillsView);

            IReactiveVariable<int> score = _gameContext.GetValue(GameContextAPI.Score);
            score.Observe(OnScoreChanged);
        }

        public void Dispose(IUIContext context) => _disposables?.Dispose();
        
        private void OnScoreChanged(int value) => _view.SetText(value.ToString());
    }
}