namespace SampleGame.AI
{
    public struct HoldPositionCommandData : ICommandData
    {
        public CommandType Type => CommandType.HoldPosition;
    }
}