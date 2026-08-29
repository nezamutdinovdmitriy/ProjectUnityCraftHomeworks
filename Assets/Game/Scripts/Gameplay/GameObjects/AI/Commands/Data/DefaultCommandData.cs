namespace SampleGame.AI
{
    public struct DefaultCommandData : ICommandData
    {
        public CommandType Type => CommandType.Default;
    }
}