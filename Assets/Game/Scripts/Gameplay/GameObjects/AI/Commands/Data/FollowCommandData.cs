namespace SampleGame.AI
{
    public readonly struct FollowCommandData : ICommandData, IHasCommandPoint
    {
        private readonly CommandPoint _point;

        public FollowCommandData(CommandPoint point) 
            => _point = point;

        public CommandType Type => CommandType.Follow;
        public CommandPoint Point => _point;
    }
}