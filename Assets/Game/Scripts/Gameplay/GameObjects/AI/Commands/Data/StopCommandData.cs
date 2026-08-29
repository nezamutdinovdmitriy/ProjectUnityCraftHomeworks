namespace SampleGame.AI
{
    public struct StopCommandData : ICommandData
    {
        public CommandType Type => CommandType.Stop;
    }
}