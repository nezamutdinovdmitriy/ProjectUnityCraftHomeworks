using UnityEngine;
using Zenject;

namespace Game.Scripts.Domain.App
{
    public class VersionProvider : IVersionProvider, IInitializable
    {
        private const string SaveVersionKey = "SaveVersion";
        
        private int _version;
        
        public void Initialize()
        {
            _version = PlayerPrefs.GetInt(SaveVersionKey, 0);
        }

        public bool IsVersionValid(int version)
        {
            if (version < 0 || version > _version)
                return false;

            return true;
        }

        public bool IsVersionValid()
        {
            throw new System.NotImplementedException();
        }

        public int GetNextVersion() => _version + 1;

        public int GetCurrentVersion() => _version;
        public void IncreaseVersion()
        {
            _version++;
            
            PlayerPrefs.SetInt(SaveVersionKey, _version);
            PlayerPrefs.Save();
        }
    }
}