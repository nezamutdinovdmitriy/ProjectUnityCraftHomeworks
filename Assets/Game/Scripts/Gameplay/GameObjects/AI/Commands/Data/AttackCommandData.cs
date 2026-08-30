namespace SampleGame.AI
{
    public readonly struct AttackCommandData : ICommandData, IHasCommandPoint
    {
        private readonly CommandPoint _point;
        
        public AttackCommandData(CommandPoint point) 
            => _point = point;

        public CommandType Type => CommandType.Attack;
        public CommandPoint Point => _point;
    }
}