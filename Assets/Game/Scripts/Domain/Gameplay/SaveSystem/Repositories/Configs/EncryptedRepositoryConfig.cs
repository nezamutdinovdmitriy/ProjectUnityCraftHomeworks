using UnityEngine;

namespace Game.Scripts.Domain.Repositories
{
    [CreateAssetMenu(
        fileName = "EncryptedRepositoryConfig",
        menuName = "Repository/New EncryptedRepositoryConfig")]
    public class EncryptedRepositoryConfig : ScriptableObject
    {
        [field: SerializeField]
        public string Key { get; private set; } = "A67B9A34EF9832A1BC9D8F7E6A5B4C32";

        [field: SerializeField]
        public string InitializationVector { get; private set; } = "abcdefghijklmnop";
    }
}