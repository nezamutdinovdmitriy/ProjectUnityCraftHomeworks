using System;
using Cysharp.Threading.Tasks;
using Game.Scripts.Domain;

namespace Game.Gameplay
{
    public class SaveLoadPresenter : IControlsPresenter
    {
        private readonly SaveManager _saveManager;
        
        public SaveLoadPresenter(SaveManager saveManager) 
            => _saveManager = saveManager;
        
        public void Save(Action<bool, int> callback) 
            => ExecuteSave(callback).Forget();

        public void Load(string version, Action<bool, int> callback) 
            => ExecuteLoad(version, callback).Forget();

        private async UniTaskVoid ExecuteSave(Action<bool, int> callback)
        {
            (bool success, int version) = await _saveManager.SaveAsync();
            
            callback?.Invoke(success, version);
        }

        private async UniTaskVoid ExecuteLoad(string version, Action<bool, int> callback)
        {
            (bool success, int loadedVersion) = await _saveManager.LoadAsync(version);
            
            callback?.Invoke(success, loadedVersion);
        }
    }
}