namespace SampleGame.AI
{
    public struct StopCommandData : ICommandData
    {
        public CommandType CommandType => CommandType.Stop;
    }
}