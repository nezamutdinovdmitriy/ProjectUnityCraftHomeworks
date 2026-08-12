using Atomic.Elements;
using Atomic.Entities;
using Game.GameEntity;

namespace Game.UI
{
    public class CharacterScorePresenter : IUIContextInit, IUIContextDispose
    {
        private readonly DisposableComposite _disposables = new();
        private readonly IReactiveVariable<int> _score;
        
        private StatView _view;

        public CharacterScorePresenter(IReactiveVariable<int> score) 
            => _score = score;

        public void Init(IUIContext context)
        {
            _view = context.GetValue(UIContextAPI.KillsView);

            _score.Observe(OnScoreChanged);
        }

        public void Dispose(IUIContext context) => _disposables?.Dispose();
        
        private void OnScoreChanged(int value) => _view.SetText(value.ToString());
    }
}