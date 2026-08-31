namespace SampleGame.AI
{
    public readonly struct DefaultCommandData : ICommandData, IHasCommandPoint
    {
        private readonly CommandPoint _point;

        public DefaultCommandData(CommandPoint point) 
            => _point = point;

        public CommandType Type => CommandType.Default;
        public CommandPoint Point => _point;
    }
}