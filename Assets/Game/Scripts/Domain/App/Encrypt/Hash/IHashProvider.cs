namespace Game.Scripts.Domain.App.Hash
{
    public interface IHashProvider
    {
        public byte[] GetHash(byte[] input);
        public bool VerifyHash(byte[] inputData, byte[] trueHash);
    }
}