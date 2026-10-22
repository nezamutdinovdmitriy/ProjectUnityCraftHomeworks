using System;
using Modules.AudioEvents;
using Unity.Collections;
using UnityEngine;

namespace Game.Presentation.Audio
{
    public sealed class AudioPlayback
    {
        private readonly AudioSystem _audioSystem;
        private readonly AudioEventKeyCache _keys = new();
        
        public AudioPlayback(AudioSystem audioSystem)
        {
            if (audioSystem == null)
                throw new ArgumentNullException(nameof(audioSystem));

            _audioSystem = audioSystem;
        }
        
        public bool Play(
            in FixedString32Bytes eventName,
            Vector3 position)
        {
            if (_audioSystem == null
                || _audioSystem.isActiveAndEnabled == false)
                return false;

            if (_keys.TryGet(in eventName, out AudioEventKey key) == false)
                return false;

            return _audioSystem.PlayEvent(key, position);
        }
    }
}