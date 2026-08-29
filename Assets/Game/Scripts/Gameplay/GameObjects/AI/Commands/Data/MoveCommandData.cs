namespace SampleGame.AI
{
    public readonly struct MoveCommandData : ICommandData
    {
        public readonly CommandPoint Point;

        public MoveCommandData(CommandPoint point) 
            => Point = point;

        public CommandType Type => CommandType.Move;
    }
}