namespace SampleGame.AI
{
    public struct HoldPositionCommandData : ICommandData
    {
        public CommandType CommandType => CommandType.Hold;
        
    }
}