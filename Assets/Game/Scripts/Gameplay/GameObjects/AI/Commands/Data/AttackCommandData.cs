namespace SampleGame.AI
{
    public struct AttackCommandData : ICommandData
    {
        public readonly CommandPoint Point;
        
        public AttackCommandData(CommandPoint point) 
            => Point = point;

        public CommandType Type => CommandType.Attack;
    }
}