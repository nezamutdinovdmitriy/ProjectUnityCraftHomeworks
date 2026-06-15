using UnityEngine;

namespace Game.Scripts.Domain.Repositories
{
    [CreateAssetMenu(
        fileName = "RemoteRepositoryConfig",
        menuName = "Repository/New RemoteRepositoryConfig")]
    public class RemoteRepositoryConfig : ScriptableObject
    {
        [Header("Main")] [SerializeField]
        private string _uri = "http://127.0.0.1:8888";

        [Header("Request Paths")] [SerializeField]
        private string _savePath = "/save?version=";

        [SerializeField]
        private string _loadPath = "/load?version=";

        public string Save(string version) => $"{_uri}{_savePath}{version}";
        public string Load(string version) => $"{_uri}{_loadPath}{version}";
    }
}