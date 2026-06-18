namespace Game.Scripts.Domain.Encrypt
{
    public interface IEncryptor
    {
        public byte[] Encrypt(byte[] plainBytes);
        public byte[] Decrypt(byte[] cipherBytes);
    }
}