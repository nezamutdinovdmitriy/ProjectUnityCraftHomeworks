namespace Atomic.Elements
{
    // TODO:
    public sealed class SmartAction : IAction
    {
        private readonly AndExpression _condition = new();
        private readonly CompositeAction _action = new();
        private readonly Event _event = new();
        
        public void Invoke()
        {
            if (_condition.Invoke())
            {
                _action.Invoke();
                _event.Invoke();
            }
        }
    }
}