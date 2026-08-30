namespace SampleGame.AI
{
    public readonly struct MoveCommandData : ICommandData, IHasCommandPoint
    {
        private readonly CommandPoint _point;

        public MoveCommandData(CommandPoint point) 
            => _point = point;

        public CommandType Type => CommandType.Move;
        public CommandPoint Point => _point;
    }
}